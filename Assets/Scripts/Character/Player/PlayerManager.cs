using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerManager : CharacterManager
    {
        private InputManager inputManager;
        private Animator animator;
        private CameraManager cameraManager;
        private PlayerStats playerStats;
        private PlayerAnimatorHandler playerAnimatorHandler;
        private PlayerLocomotion playerLocomotion;

        [Header("User Interface")]
        public GameObject interactablePopUp;
        public GameObject itemPopUp;
        private float itemPopUpTimerCurrent = 0;
        private float itemPopUpTimerMax = 3;
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

        public bool isBlocking;
        public bool isBlockingLeftHand;
        public bool isBlockingRightHand;
        public bool isBlockingTwoHand;

        private void Awake()
        {
            cameraManager = FindObjectOfType<CameraManager>();
            inputManager = GetComponent<InputManager>();
            animator = GetComponentInChildren<Animator>();
            playerStats = GetComponent<PlayerStats>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
            interactableUI = FindObjectOfType<InteractableUI>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isInvulnerable = animator.GetBool("isInvulnerable");

            animator.SetBool("isBlockingRightHand", isBlockingRightHand);
            animator.SetBool("isBlockingLeftHand", isBlockingLeftHand);
            animator.SetBool("isBlockingTwoHand", isBlockingTwoHand);
            animator.SetBool("isAirborne", isAirborne);
            animator.SetBool("isDead", playerStats.isDead);

            SetIsBlocking(); // This may need to go somewhere else for readability

            inputManager.flag_Roll = false;
            inputManager.flag_Sprint = false;
            inputManager.TickInput(delta);

            playerAnimatorHandler.canRotate = animator.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();

            playerStats.PassiveRegenerateHealth();
            playerStats.PassiveRegenerateStamina();
            playerStats.PassiveRegenerateMana();

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleRotation(delta);
        }

        private void LateUpdate()
        {
            // Set inputs and flags to false so they can only be called once per frame
            #region Check for Inputs
            inputManager.flag_Roll = false;
            inputManager.input_RT = false;
            inputManager.input_LT = false;
            inputManager.input_Dpad_Up = false;
            inputManager.input_Dpad_Down = false;
            inputManager.input_Dpad_Left = false;
            inputManager.input_Dpad_Right = false;
            inputManager.input_A = false;
            inputManager.input_Jump = false;
            inputManager.input_LeftPanel = false;
            #endregion

            float delta = Time.deltaTime;

            if (cameraManager != null)
            {
                cameraManager.HandleCameraZoom(delta, inputManager.mouseScrollY);
                cameraManager.FollowTarget(delta);
                cameraManager.HandleCameraRotation(delta, inputManager.mouseX, inputManager.mouseY);
            }

            if (isAirborne)
                playerLocomotion.InAirTimer = playerLocomotion.InAirTimer + Time.deltaTime;
        }

        private bool SetIsBlocking()
        {
            if (isBlockingLeftHand == true || isBlockingRightHand == true || isBlockingTwoHand == true)
                isBlocking = true;

            else
                isBlocking = false;

            return isBlocking;
        }

        #region Player Interactions
        public void CheckForInteractableObject()    // this entire thing will probabably be moved to its own class
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f))
            {
                if (isInteracting)
                    return;

                if (hit.collider.tag != "Interactable")
                    return;

                Debug.Log("Collided with an interactable object!");
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject == null)
                     return;

                string interactableText = interactableObject.interactableText;
                interactableUI.interactableText.text = interactableText;
                interactablePopUp.SetActive(true);

                if (inputManager.input_A && !isInteracting)
                {
                    hit.collider.GetComponent<Interactable>().Interact(this);
                    interactablePopUp.SetActive(false); // Bug Fix. Interactable text not automatically disabling
                }
            }

            else
            {
                // disable the interatable popup if there is no interactable object in range
                if (interactablePopUp != null)
                {
                    interactablePopUp.SetActive(false);
                }
            }

            //when all of this functionality is put into its own class, must allow for multiple item popups at once
            // check if there is an item popup currently active
            if (itemPopUp != null)
            {
                // if so, start the timer
                itemPopUpTimerCurrent -= Time.deltaTime;

                // if the timer reaches zero, disable the item popup and restart the timer for next time
                if (itemPopUpTimerCurrent <= 0)
                {
                    itemPopUp.SetActive(false);
                    itemPopUpTimerCurrent = itemPopUpTimerMax;
                }
            }
        }

        public void OpenChestInteraction(Transform _playerStandsHereWhenOpeningChest)
        {
            playerLocomotion.rigidbody.linearVelocity = Vector3.zero; // Stops the player from ice skating while looting
            transform.position = _playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorHandler.PlayTargetAnimation("Open_Chest", true);
        }
        #endregion
    }
}