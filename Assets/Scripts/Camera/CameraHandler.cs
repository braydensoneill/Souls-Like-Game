using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CameraHandler : MonoBehaviour
    {
        private InputHandler inputHandler;
        private PlayerManager playerManager;
        public static CameraHandler singleton;
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
        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;

        [Header("Camera Pivot")]
        public float minimumPivot = -35;
        public float maximumPivot = 35;
        public float cameraPivotXNormal = 0.4f;
        public float cameraPivotXLeftPanel = -1.4f;
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
            ignoreLayers = ~(1 << 0 | 1 << 8 | 1 << 10 | 1 << 11 | 1 << 12 | 1 << 13);    // Don't ignore layer 9 (environment layer)
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            uiManager = FindObjectOfType<UIManager>();
        }

        private void Start()
        {
            environmentLayer = LayerMask.NameToLayer("Environment");
        }

        private void Update()
        {
            HandleActiveLeftPanelCamera();  // I don't know where to put this yet
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

        public void HandleCameraRotation(float _delta, float _mouseXInput, float _mouseYInput)
        {
            // If not currently locked on / using the menu
            if (currentLockOnTarget == null && inputHandler.flag_LockOn == false)
            {
                if(inputHandler.flag_LeftPanel == false)
                {
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
            }

            // If currently locked on
            else
            {
                float velocity = 0;
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

        private void HandleCameraCollisions(float _delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(
                cameraPivotTransform.position,
                cameraSphereRadius,
                direction,
                out hit,
                Mathf.Abs(targetPosition),
                ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, _delta / 0.02f);
            cameraTransform.localPosition = cameraTransformPosition;
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
                                // inputHandler.flag_LockOn = false;
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

                if(inputHandler.flag_LockOn)
                {
                    // TBD - Comment using this video as reference: https://www.youtube.com/watch?v=SH87d9utAmg&ab_channel=SebastianGraves

                    Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[i].transform.position);
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

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(cameraPivotXNormal, pivotPositionYLocked);
            Vector3 newUnlockedPosition = new Vector3(cameraPivotXNormal, pivotPositionYNormal);

            if(currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition,
                    newLockedPosition,
                    ref velocity,
                    Time.deltaTime);
            }

            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition,
                    newUnlockedPosition,
                    ref velocity,
                    Time.deltaTime);
            }
        }

        public void HandleActiveLeftPanelCamera()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 leftPanelCameraOn = new Vector3(cameraPivotXLeftPanel, pivotPositionYLeftPanel);
            Vector3 leftPanelCameraOff = new Vector3(cameraPivotXNormal, pivotPositionYNormal);

            if (uiManager.leftPanel.activeSelf == true)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition,
                    leftPanelCameraOn,
                    ref velocity,
                    Time.deltaTime);
            }

            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition,
                    leftPanelCameraOff,
                    ref velocity,
                    Time.deltaTime);
            }

        }
    }
}
