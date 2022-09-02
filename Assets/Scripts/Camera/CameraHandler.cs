using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CameraHandler : MonoBehaviour
    {
        public static CameraHandler singleton;

        [Header("General")]
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;

        private Transform _myTransform;
        private Vector3 _cameraTransformPosition;
        public LayerMask ignoreLayers;
        private Vector3 _cameraFollowVelocity = Vector3.zero;

        [Header("Camera Movement")]
        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float _targetPosition;
        private float _defaultPosition;
        private float _lookAngle;
        private float _pivotAngle;

        [Header("Camera Pivot")]
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        [Header("Camera Collision")]
        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        private void Awake()
        {
            singleton = this;
            _myTransform = transform;
            _defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget(float delta)
        {
            Vector3 _targetPosition = Vector3.SmoothDamp(
                _myTransform.position, 
                targetTransform.position, 
                ref _cameraFollowVelocity,
                delta / followSpeed);

            _myTransform.position = _targetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            _lookAngle += (mouseXInput * lookSpeed) / delta;
            _pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivot, maximumPivot);

            Vector3 _rotation = Vector3.zero;
            _rotation.y = _lookAngle;
            Quaternion _targetRotation = Quaternion.Euler(_rotation);
            _myTransform.rotation = _targetRotation;

            _rotation = Vector3.zero;
            _rotation.x = _pivotAngle;

            _targetRotation = Quaternion.Euler(_rotation);
            cameraPivotTransform.localRotation = _targetRotation;
        }

        private void HandleCameraCollisions(float delta)
        {
            _targetPosition = _defaultPosition;
            RaycastHit _hit;
            Vector3 _direction = cameraTransform.position - cameraPivotTransform.position;
            _direction.Normalize();

            if (Physics.SphereCast(
                cameraPivotTransform.position,
                cameraSphereRadius,
                _direction,
                out _hit,
                Mathf.Abs(_targetPosition),
                ignoreLayers))
            {
                float _dis = Vector3.Distance(cameraPivotTransform.position, _hit.point);
                _targetPosition = -(_dis - cameraCollisionOffset);
            }

            if(Mathf.Abs(_targetPosition) < minimumCollisionOffset)
            {
                _targetPosition = -minimumCollisionOffset;
            }

            _cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, _targetPosition, delta / 0.02f);
            cameraTransform.localPosition = _cameraTransformPosition;
        }
    }
}
