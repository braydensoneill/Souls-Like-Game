using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Direction")]
        public float horizontal;
        public float vertical;

        [Header("Mouse")]
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_input;
        public bool rb_input;
        public bool rt_input;

        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public float rollInputTimer;

        private PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;

        private Vector2 movementInput;
        private Vector2 cameraInput;

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
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
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
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            b_input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (b_input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }

            else
            {
                if(rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.RB.performed += i => rb_input = true;
            inputActions.PlayerActions.RT.performed += i => rt_input = true;

            if(rb_input)
            {
                if(playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }

                else
                {
                    if (playerManager.canDoCombo || playerManager.isInteracting)
                        return;

                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            if (rt_input)
            {
                if (playerManager.canDoCombo || !playerManager.isInteracting)
                {
                    comboFlag = true;
                    playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
                }
            }
        }
    }
}
