using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerManager : CharacterManager
    {
        private InputHandler inputHandler;
        private Animator animator;
        private CameraHandler cameraHandler;
        private PlayerStats playerStats;
        private PlayerLocomotion playerLocomotion;

        [Header("User Interface")]
        public GameObject interactablePopUp;
        public GameObject itemPopUp;
        private InteractableUI interactableUI;

        [Header("Player Flags")]
        public bool isInteracting;
        public bool isSprinting;
        public bool isAirborne;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;

        private void Awake()
        {
            backStabCollider = GetComponentInChildren<BackStabCollider>();
        }

        // Start is called before the first frame update
        void Start()
        {
            cameraHandler = CameraHandler.singleton;    // do this here or findobjectoftype in awake?
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
            playerStats = GetComponent<PlayerStats>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        // Update is called once per frame
        void Update()
        {
            float _delta = Time.deltaTime;

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isInvulnerable = animator.GetBool("isInvulnerable");
            animator.SetBool("isAirborne", isAirborne);

            inputHandler.flag_Roll = false;
            inputHandler.flag_Sprint = false;

            inputHandler.TickInput(_delta);

            playerLocomotion.HandleRollingAndSprinting(_delta);
            playerLocomotion.HandleJumping();
            playerStats.RegenerateStamina();

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float _delta = Time.fixedDeltaTime;

            playerLocomotion.HandleMovement(_delta);
            playerLocomotion.HandleRollingAndSprinting(_delta);
            playerLocomotion.HandleFalling(_delta, playerLocomotion.moveDirection);
        }

        private void LateUpdate()
        {
            // Set inputs and flags to false so they can only be called once per frame
            #region Check for Inputs
            inputHandler.flag_Roll = false;
            inputHandler.input_RT = false;
            inputHandler.input_RB = false;
            inputHandler.input_Dpad_Up = false;
            inputHandler.input_Dpad_Down = false;
            inputHandler.input_Dpad_Left = false;
            inputHandler.input_Dpad_Right = false;
            inputHandler.input_A = false;
            inputHandler.input_Jump = false;
            inputHandler.input_Inventory = false;
            #endregion

            float _delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(_delta);
                cameraHandler.HandleCameraRotation(_delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isAirborne)
                playerLocomotion.InAirTimer = playerLocomotion.InAirTimer + Time.deltaTime;
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if(hit.collider.tag == "Interactable" && !isInteracting)
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactablePopUp.SetActive(true);

                        if (inputHandler.input_A && !isInteracting)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }

            else
            {
                if(interactablePopUp != null)
                {
                    interactablePopUp.SetActive(false);
                }

                if(itemPopUp != null && inputHandler.input_A)
                {
                    itemPopUp.SetActive(false);
                }
            }
        }
    }
}