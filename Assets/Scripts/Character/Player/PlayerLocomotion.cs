﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{ 
    public class PlayerLocomotion : MonoBehaviour
    {
        [Header("References")]
        public new Rigidbody rigidbody;
        private PlayerManager playerManager;
        private PlayerStats playerStats;
        private Transform cameraObject;
        private CameraManager cameraManager;
        private InputManager inputManager;
        [HideInInspector] public Transform myTransform;
        [HideInInspector] public PlayerAnimatorHandler playerAnimatorHandler;

        // This should be moved to the References section and the references section should be renamed "general"
        [Header("General")]
        public GameObject normalCamera;

        [Header("Ground and Air Detection Stats")]
        public float InAirTimer;
        [SerializeField] private float _groundDetectionRayStartPoint = 0.5f;
        [SerializeField] private float _minimumDistanceNeededToBeginFall = 1f;
        [SerializeField] private float _groundDirectionRayDistance = 0.2f;
        private LayerMask _ignoreForGroundCheck;

        [Header("Movement Stats")]
        public Vector3 moveDirection;
        [SerializeField] private float _movementSpeed = 5;
        [SerializeField] private float _walkSpeed = 1;
        [SerializeField] private float _sprintSpeed = 7;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _fallSpeed = 100;
        [SerializeField] private float _fallForce = 5;

        [Header("Stamina Costs")]
        [SerializeField] private float rollStaminaCost = 20;
        [SerializeField] private float backstepStaminaCost = 15;
        [SerializeField] private float sprintCost = 1;

        [Header("Character Collision")]
        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        private void Awake()
        {
            cameraManager = FindObjectOfType<CameraManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            rigidbody = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
            playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            cameraObject = Camera.main.transform;
            myTransform = transform;

            playerAnimatorHandler.Initialise();

            playerManager.isGrounded = true;
            _ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }

        #region Movement
        Vector3 _normalVector;
        Vector3 _targetPosition;

        public void HandleRotation(float _delta)
        {
            if (playerAnimatorHandler.canRotate)
            {
                // Check if the player is currently locked on to a target
                if (inputManager.flag_LockOn)
                {
                    // Sprinting/Rolling while locked on
                    if (inputManager.flag_Sprint || inputManager.flag_Roll)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = cameraManager.cameraTransform.forward * inputManager.vertical;
                        targetDirection += cameraManager.cameraTransform.right * inputManager.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, _rotationSpeed * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }

                    // Not sprinting or rolling while locked on
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = cameraManager.currentLockOnTarget.transform.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, _rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                }

                // Check if the player is NOT currently locked on to a target
                else
                {
                    Vector3 _targetDir = Vector3.zero;
                    float _moveOverride = inputManager.moveAmount;

                    _targetDir = cameraObject.forward * inputManager.vertical;
                    _targetDir += cameraObject.right * inputManager.horizontal;

                    _targetDir.Normalize();
                    _targetDir.y = 0;

                    if (_targetDir == Vector3.zero)
                        _targetDir = myTransform.forward;

                    float _rs = _rotationSpeed;

                    Quaternion _tr = Quaternion.LookRotation(_targetDir);
                    Quaternion _targetRotation = Quaternion.Slerp(myTransform.rotation, _tr, _rs * _delta);

                    myTransform.rotation = _targetRotation;
                }
            }
        }

        public void HandleMovement(float delta)
        {
            if (inputManager.flag_Roll)
                return;

            if (playerManager.isInteracting)
                return;

            moveDirection = cameraObject.forward * inputManager.vertical;
            moveDirection += cameraObject.right * inputManager.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = _movementSpeed;

            if (inputManager.moveAmount < 0.5)
            {
                moveDirection *= _walkSpeed;
                playerManager.isSprinting = false;
            }

            else if (inputManager.flag_Sprint && inputManager.moveAmount > 0.5)
            {
                speed = _sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerStats.TakeStaminaDamage(sprintCost);
            }

            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }

            Vector3 _projectedVelocity = Vector3.ProjectOnPlane(moveDirection, _normalVector);
            rigidbody.linearVelocity = _projectedVelocity;

            if(inputManager.flag_LockOn && inputManager.flag_Sprint == false)
            {
                playerAnimatorHandler.UpdateAnimatorValues(inputManager.vertical, inputManager.horizontal, playerManager.isSprinting);

            }

            else
            {
                playerAnimatorHandler.UpdateAnimatorValues(inputManager.moveAmount, 0, playerManager.isSprinting);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            // Unless the player is currently interacting
            if (playerAnimatorHandler.animator.GetBool("isInteracting"))
                return;

            // Unless the player has insufficient stamina
            if (playerStats.stamina_Current <= 0)
                return;

            if(inputManager.flag_Roll)
            {
                moveDirection = cameraObject.forward * inputManager.vertical;
                moveDirection += cameraObject.right * inputManager.horizontal;

                if(inputManager.moveAmount > 0)
                {
                    playerAnimatorHandler.PlayTargetAnimation("Roll", true);
                    moveDirection.y = 0;
                    Quaternion _rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = _rollRotation;
                    playerStats.TakeStaminaDamage(rollStaminaCost);
                }

                else
                {
                    playerAnimatorHandler.PlayTargetAnimation("Backstep", true);
                    playerStats.TakeStaminaDamage(backstepStaminaCost);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit _hit;
            Vector3 _origin = myTransform.position;
            _origin.y += _groundDetectionRayStartPoint;

            if(Physics.Raycast(_origin, myTransform.forward, out _hit, 0.4f))
                moveDirection = Vector3.zero;

            if(playerManager.isAirborne)
            {
                rigidbody.AddForce(-Vector3.up * _fallSpeed);
                rigidbody.AddForce(moveDirection * _fallSpeed / _fallForce);
            }

            Vector3 _dir = moveDirection;
            _dir.Normalize();
            _origin = _origin + _dir * _groundDirectionRayDistance;

            _targetPosition = myTransform.position;
            Debug.DrawRay(_origin, -Vector3.up * _minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);

            if(Physics.Raycast(_origin, -Vector3.up, out _hit, _minimumDistanceNeededToBeginFall, _ignoreForGroundCheck))
            {
                _normalVector = _hit.normal;
                Vector3 _tp = _hit.point;
                playerManager.isGrounded = true;
                _targetPosition.y = _tp.y;

                if(playerManager.isAirborne)
                {
                    if(InAirTimer > 0.5f)
                    {
                        Debug.Log("Air Time: " + InAirTimer);
                        playerAnimatorHandler.PlayTargetAnimation("Land", true);
                        InAirTimer = 0;
                    }

                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Empty", true);
                        InAirTimer = 0;
                    }

                    playerManager.isAirborne = false;
                }
            }

            else
            {
                if(playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if(playerManager.isAirborne == false)
                {
                    if(playerManager.isInteracting == false)
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Fall", true);
                    }

                    Vector3 _vel = rigidbody.linearVelocity;
                    _vel.Normalize();
                    rigidbody.linearVelocity = _vel * (_movementSpeed / 2);
                    playerManager.isAirborne = true;
                }
            }

            if(playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, _targetPosition, Time.deltaTime / 0.1f);
            }

            else
            {
                myTransform.position = _targetPosition;
            }
        }
        
        public void HandleJumping()
        {
            // Unless the player is currently interacting
            if (playerManager.isInteracting)
                return;

            // Unless the player has insufficient stamina
            if (playerStats.stamina_Current <= 0)
                return;

            if (inputManager.input_Jump)
            {
                if(inputManager.moveAmount > 0)
                {
                    // Direction XZ
                    moveDirection = cameraObject.forward * inputManager.vertical * inputManager.moveAmount;
                    moveDirection += cameraObject.right * inputManager.horizontal * inputManager.moveAmount;

                    // Animations
                    playerAnimatorHandler.PlayTargetAnimation("Jump", true);
                    moveDirection.y = 0;

                    // Rotation
                    Quaternion _jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = _jumpRotation;
                }    
            }
        }
        #endregion
    }
}
