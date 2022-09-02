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

            CheckForInteractableObject();
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
            inputHandler.input_A = false;
            #endregion

            if (isAirborne)
                playerLocomotion.InAirTimer = playerLocomotion.InAirTimer + Time.deltaTime;
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        // set the ui text to the interactable object;s text
                        // set the text pup up to true
                        
                        if(inputHandler.input_A)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
        }
    }
}