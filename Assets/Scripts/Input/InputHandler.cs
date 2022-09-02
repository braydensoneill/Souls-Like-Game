using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class InputHandler : MonoBehaviour
    {
        // References
        private PlayerControls inputActions;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        private PlayerManager playerManager;

        // Direction Variables
        [Header("Direction")]
        public float horizontal;
        public float vertical;

        // Mouse Variables
        [Header("Mouse")]
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        // Input Variables
        [Header("Inputs")]
        public bool input_B;
        public bool input_A;
        public bool input_RB;
        public bool input_RT;
        public bool input_Dpad_Up;
        public bool input_Dpad_Down;
        public bool input_Dpad_Left;
        public bool input_Dpad_Right;

        // Flag Variables
        [Header("Flags")]
        public bool flag_Roll;
        public bool flag_Sprint;
        public bool flag_Combo;

        // Timer Variables
        [Header("Timers")]
        public float timer_Roll_Input;

        // Input Vectors
        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
        }

        public void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => _movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotInput(delta);
            HandleInteractInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = _movementInput.x;
            vertical = _movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            input_B = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (input_B)
            {
                timer_Roll_Input += delta;
                flag_Sprint = true;
            }

            else
            {
                if(timer_Roll_Input > 0 && timer_Roll_Input < 0.5f)
                {
                    flag_Sprint = false;
                    flag_Roll = true;
                }

                timer_Roll_Input = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.RB.performed += i => input_RB = true;
            inputActions.PlayerActions.RT.performed += i => input_RT = true;

            if(input_RB)
            {
                if(playerManager.canDoCombo)
                {
                    flag_Combo = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    flag_Combo = false;
                }

                else
                {
                    if (playerManager.canDoCombo || playerManager.isInteracting)
                        return;

                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            if (input_RT)
            {
                if (playerManager. canDoCombo || !playerManager.isInteracting)
                {
                    flag_Combo = true;
                    playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
                    flag_Combo = false;
                }
            }
        }

        private void HandleQuickSlotInput(float delta)
        {
            inputActions.PlayerQuickslots.DPadRight.performed += inputActions => input_Dpad_Right = true;
            inputActions.PlayerQuickslots.DPadLeft.performed += inputActions => input_Dpad_Left = true;

            if(input_Dpad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }

            if(input_Dpad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleInteractInput(float delta)
        {
            inputActions.PlayerActions.Interact.performed += i => input_A = true;
            if (input_A == true)
                Debug.Log("Pressed F");
        }
    }
}
