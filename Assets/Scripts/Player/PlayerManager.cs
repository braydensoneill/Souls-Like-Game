using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;

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
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");

            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;

            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            #region Check for Inputs
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.rb_input = false;
            inputHandler.rt_input = false;
            inputHandler.d_pad_up = false;
            inputHandler.d_pad_down = false;
            inputHandler.d_pad_left = false;
            inputHandler.d_pad_right = false;
            #endregion

            if (isAirborne)
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }
}