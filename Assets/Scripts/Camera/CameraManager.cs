using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CameraManager : MonoBehaviour
    {
        private InputManager inputManager;
        private PlayerManager playerManager;
        public static CameraManager singleton;
        private UIManager uiManager;

        [Header("General")]
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;

        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        public LayerMask environmentLayer;
        private Vector3 _cameraFollowVelocity = Vector3.zero;

        [Header("Camera Movement")]
        public float lookSpeed = 0.0015f;   // camera sensitivity (left/right)
        public float followSpeed = 0.1f;    // camera 'catch up' to player
        public float pivotSpeed = 0.00125f;    // camera sensitivity (up/down)

        [Header("Camera Zoom")]
        //[SerializeField] private float cameraZoomCurrent;
        public float cameraZoomIntensity = 0.5f;
        private float cameraZoomMinimum = -2;
        private float cameraZoomMaximum = -4;

        private float collisionTargetPosition;
        private float defaultPosition;
        private float zoomedTargetPosition;
        private float lookAngle;
        private float pivotAngle;

        [Header("Camera Pivot")]
        public float minimumPivot = -90;    // how far can i look up (was -35)
        public float maximumPivot = 90;     // how far can i look down (was 35)
        public float cameraPivotXNormal = 0.4f;
        public float cameraPivotXLeftPanel = -0.8f;
        public float pivotPositionYLocked = 1.65f;
        public float pivotPositionYNormal = 1.4f;
        public float pivotPositionYLeftPanel = 1;

        [Header("Camera Collision")]
        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        [Header("Target Lock On")]
        public float maximumLockOnDistance = 30;
        public CharacterManager currentLockOnTarget;
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;
        private List<CharacterManager> availableTargets = new List<CharacterManager>();

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 0 | 1 << 8 | 1 << 10 | 1 << 11 | 1 << 12 | 1 << 13 | 1 << 14);    // Don't ignore layer 9 (environment layer)
            inputManager = FindObjectOfType<InputManager>();
            playerManager = FindObjectOfType<PlayerManager>();
            uiManager = FindObjectOfType<UIManager>();
        }

        private void Start()
        {
            environmentLayer = LayerMask.NameToLayer("Environment");
        }

        private void Update()
        {
            /* Putting these here rather than HandleLockOnInput() and 
             *  HandleLeftPanelInput() may have fucked up the direction
             *  of rolling while locked on to a target.
             */
            HandleLockOnCameraPosition();
            HandleLeftPanelCameraPosition();
        }

        public void FollowTarget(float _delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(
                myTransform.position, 
                targetTransform.position, 
                ref _cameraFollowVelocity,
                _delta / followSpeed);

            myTransform.position = targetPosition;

            HandleCameraCollisions(_delta);
        }

        public void HandleCameraZoom(float _delta, float _mouseScrollY)
        {
            if (inputManager.flag_LeftPanel == true)
                return;

            zoomedTargetPosition = defaultPosition + (_mouseScrollY * cameraZoomIntensity);

            if (zoomedTargetPosition > cameraZoomMinimum)
                zoomedTargetPosition = cameraZoomMinimum;

            if (zoomedTargetPosition < cameraZoomMaximum)
                zoomedTargetPosition = cameraZoomMaximum;

            if (collisionTargetPosition < zoomedTargetPosition)
            {
                cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, zoomedTargetPosition, _delta / 0.02f);
                cameraTransform.localPosition = cameraTransformPosition;
            }
        }

        private void HandleCameraCollisions(float _delta)
        {
            collisionTargetPosition = zoomedTargetPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(
                cameraPivotTransform.position,
                cameraSphereRadius,
                direction,
                out hit,
                Mathf.Abs(collisionTargetPosition),
                ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                collisionTargetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(collisionTargetPosition) < minimumCollisionOffset)
            {
                collisionTargetPosition = -minimumCollisionOffset;
            }

            if (collisionTargetPosition >= zoomedTargetPosition)
            {
                cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, collisionTargetPosition, _delta / 0.02f);
                cameraTransform.localPosition = cameraTransformPosition;
            }
        }

        public void HandleCameraRotation(float _delta, float _mouseXInput, float _mouseYInput)
        {
            // If not currently locked on / using the menu
            if (currentLockOnTarget == null && inputManager.flag_LockOn == false)
            {
                if (inputManager.flag_LeftPanel == true)
                    return;

                lookAngle += (_mouseXInput * lookSpeed) / _delta;
                pivotAngle -= (_mouseYInput * pivotSpeed) / _delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }

            // If currently locked on
            else
            {
                Vector3 dir = currentLockOnTarget.transform.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if(character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if(character.transform.root != targetTransform.transform.root && 
                        viewableAngle > -50 && 
                        viewableAngle < 50 && 
                        distanceFromTarget <= maximumLockOnDistance)
                    {
                        if(Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                            if (hit.transform.gameObject.layer == environmentLayer.value)
                            {
                                // cannot lock on to target, object in the way
                                // inputManager.flag_LockOn = false;
                            }

                            else
                            {
                                availableTargets.Add(character);
                            }
                        }
                    }
                }
            }

            for(int i = 0; i < availableTargets.Count; i++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[i].transform.position);
            
                if(distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[i];
                }

                if(inputManager.flag_LockOn)
                {
                    // TBD - Comment using this video as reference: https://www.youtube.com/watch?v=SH87d9utAmg&ab_channel=SebastianGraves

                    Vector3 relativeEnemyPosition = inputManager.transform.InverseTransformPoint(availableTargets[i].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    // check if the new enemy is left of the currentely locked on target
                    if (relativeEnemyPosition.x <= 0.00 && 
                        distanceFromLeftTarget > shortestDistanceOfLeftTarget && 
                        availableTargets[i] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[i];
                    }

                    else if(relativeEnemyPosition.x >= 0.00 && 
                        distanceFromRightTarget < shortestDistanceOfRightTarget &&
                        availableTargets[i] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[i];
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            currentLockOnTarget = null;
            nearestLockOnTarget = null;
        }

        public void HandleLockOnCameraPosition()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newPositionLockedCam = new Vector3(cameraPivotTransform.localPosition.x, pivotPositionYLocked);
            Vector3 newPositionUnlockedCam = new Vector3(cameraPivotTransform.localPosition.x, pivotPositionYNormal);

            if(currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition,
                    newPositionLockedCam,
                    ref velocity,
                    Time.deltaTime);
            }

            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition,
                    newPositionUnlockedCam,
                    ref velocity,
                    Time.deltaTime);
            }
        }

        public void HandleLeftPanelCameraPosition()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newPositionLeftPanelOn = new Vector3(cameraPivotXLeftPanel, pivotPositionYLeftPanel);
            Vector3 newPositionRightPanelOff = new Vector3(cameraPivotXNormal, pivotPositionYNormal);

            if (uiManager.leftPanel.activeSelf == true)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition,
                    newPositionLeftPanelOn,
                    ref velocity,
                    Time.deltaTime);
            }

            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition,
                    newPositionRightPanelOff,
                    ref velocity,
                    Time.deltaTime);
            }
        }

        public float getCameraZoomMaximum()
        {
            return cameraZoomMaximum;
        }

        public float getCameraZoomMinimum()
        {
            return cameraZoomMinimum;
        }

        public float getCameraZoomDifference()
        {
            return System.Math.Abs(getCameraZoomMaximum()) - System.Math.Abs(getCameraZoomMinimum());
        }
    }
}
