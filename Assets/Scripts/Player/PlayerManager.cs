using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerManager : MonoBehaviour
    {
        private InputHandler inputHandler;
        private Animator animator;
        private CameraHandler cameraHandler;
        private PlayerLocomotion playerLocomotion;

        [Header("Player Flags")]
        public bool isInteracting;
        public bool isSprinting;
        public bool isAirborne;
        public bool isGrounded;
        public bool canDoCombo;

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            cameraHandler = CameraHandler.singleton;
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        // Update is called once per frame
        void Update()
        {
            float _delta = Time.deltaTime;

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");

            inputHandler.flag_Roll = false;
            inputHandler.flag_Sprint = false;

            inputHandler.TickInput(_delta);
            playerLocomotion.HandleMovement(_delta);
            playerLocomotion.HandleRollingAndSprinting(_delta);
            playerLocomotion.HandleFalling(_delta, playerLocomotion.moveDirection);
        }

        private void FixedUpdate()
        {
            float _delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(_delta);
                cameraHandler.HandleCameraRotation(_delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            #region Check for Inputs
            inputHandler.flag_Roll = false;
            inputHandler.flag_Sprint = false;
            inputHandler.input_RB = false;
            inputHandler.input_RT = false;
            inputHandler.input_Dpad_Up = false;
            inputHandler.input_Dpad_Down = false;
            inputHandler.input_Dpad_Left = false;
            inputHandler.input_Dpad_Right = false;
            #endregion

            if (isAirborne)
                playerLocomotion.InAirTimer = playerLocomotion.InAirTimer + Time.deltaTime;
        }
    }
}