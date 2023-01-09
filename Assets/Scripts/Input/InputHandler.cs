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
        private PlayerStats playerStats;
        private PlayerWeaponSlotManager weaponSlotManager;
        private CameraHandler cameraHandler;
        private UIManager uiManager;
        private PlayerAnimatorHandler playerAnimatorHandler;

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
        public bool input_Y;
        public bool input_RT;
        public bool input_LT;
        public bool input_RB;
        public bool input_RBHold;

        public bool input_LB;
        public bool input_CriticalAttack;

        public bool input_Jump;
        public bool input_LeftPanel;
        public bool input_LockOn;
        public bool input_Right_Stick_Right;
        public bool input_Right_Stick_Left;
        public bool input_Dpad_Up;
        public bool input_Dpad_Down;
        public bool input_Dpad_Left;
        public bool input_Dpad_Right;

        // Flag Variables
        [Header("Flags")]
        public bool flag_Roll;
        public bool flag_TwoHand;
        public bool flag_Sprint;
        public bool flag_Combo;
        public bool flag_LeftPanel;
        public bool flag_LockOn;

        // Timer Variables
        [Header("Timers")]
        public float timer_Roll_Input;
        public float timer_RB_Input;
        public float timer_LB_Input;

        [Header("Backstab")]
        public Transform criticalAttackRayCastStartPoint;

        // Input Vectors
        private Vector2 movementInput;
        private Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            weaponSlotManager = GetComponentInChildren<PlayerWeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        }

        private void Update()
        {
            //Debug.Log("timer_RB: " + timer);
        }

        public void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

                inputActions.PlayerActions.RT.performed += i => input_RT = true;
                inputActions.PlayerActions.LT.performed += i => input_LT = true;
                inputActions.PlayerActions.RB.performed += i => input_RB = true;    // may use cancelled here in the future for heavy attacks
                inputActions.PlayerActions.RB.canceled += i => input_RB = false;

                inputActions.PlayerActions.LB.performed += i => input_LB = true;
                inputActions.PlayerActions.LB.canceled += i => input_LB = false;

                inputActions.PlayerQuickslots.DPadRight.performed += inputActions => input_Dpad_Right = true;
                inputActions.PlayerQuickslots.DPadLeft.performed += inputActions => input_Dpad_Left = true;
                
                inputActions.PlayerActions.Interact.performed += i => input_A = true;
                inputActions.PlayerActions.Jump.performed += i => input_Jump = true;
                inputActions.PlayerActions.Roll.performed += i => input_B = true;
                inputActions.PlayerActions.Roll.canceled += i => input_B = false;    // Using cancelled here becase sprint uses the same keybind (but holding).
                inputActions.PlayerActions.LeftWindow.performed += i => input_LeftPanel = true;
                inputActions.PlayerActions.LockOn.performed += i => input_LockOn = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => input_Right_Stick_Right = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => input_Right_Stick_Left = true;
                inputActions.PlayerActions.Y.performed += i => input_Y = true;
                inputActions.PlayerActions.CriticalAttack.performed += inputActions => input_CriticalAttack = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float _delta)
        {
            HandleMoveInput(_delta);
            HandleRollInput(_delta);
            HandleWeaponInput(_delta);
            HandleQuickSlotInput();
            HandleLeftPanelInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleCriticalAttackInput();
        }

        private void HandleMoveInput(float _delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float _delta)
        {
            if (input_B)
            {
                timer_Roll_Input += _delta;

                if(playerStats.stamina_Current <= 0)
                {
                    input_B = false;
                    flag_Sprint = false;
                }

                if(moveAmount > 0.5f && playerStats.stamina_Current > 0)
                {
                    flag_Sprint = true;
                }
            }

            else
            {
                flag_Sprint = false;

                if(timer_Roll_Input > 0 && timer_Roll_Input < 0.5f)
                {
                    flag_Sprint = false;
                    flag_Roll = true;
                }

                timer_Roll_Input = 0;
            }
        }

        private void HandleWeaponInput(float _delta)
        {
            // Disable attacks if menu options are open
            if (uiManager.leftPanel.activeSelf == true || uiManager.selectWindow.activeSelf == true)
                return;

            // Right hand weapon input
            if (input_RB && playerInventory.rightWeapon.isMeleeWeapon)
                timer_RB_Input += _delta;

            else if (input_RB && playerInventory.rightWeapon.isMeleeWeapon == false)
                playerAttacker.HandleRBAction();

            else
            {
                if (playerInventory.rightWeapon.isMeleeWeapon)
                    playerAttacker.HandleRBAction(timer_RB_Input);

                playerManager.isBlockingRightHand = false;
            }
                

            // Left hand weapon input
            if (input_LB)
            {
                playerAttacker.HandleLBAction();
            }

            else
            {
                timer_LB_Input = 0;
                playerManager.isBlockingLeftHand = false;
            }
                
           
            // Change this to if the player attacks while blocking
            if (input_LT)   
            {
                if (flag_TwoHand)
                {
                    // if two handed weapon art
                }

                else
                {
                    playerAttacker.HandleLTAction();
                }
            }
        }

        private void HandleQuickSlotInput()
        {
            if(input_Dpad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }

            if(input_Dpad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleLeftPanelInput()
        {
            if(input_LeftPanel)
            {
                flag_LeftPanel = !flag_LeftPanel;

                if(flag_LeftPanel)
                {
                    uiManager.UpdateUI();
                    uiManager.OpenLeftPanelWindow();
                    uiManager.hudWindow.SetActive(false);
                }
                    
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseLeftPanelWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            // Lock on to a target if you are not currently locked on to one
            if(input_LockOn && flag_LockOn == false)
            {

                input_LockOn = false;
                cameraHandler.HandleLockOn();

                if(cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    flag_LockOn = true;
                }
            }

            // Stop locking on to current target
            else  if (input_LockOn && flag_LockOn == true)
            {
                input_LockOn = false;
                flag_LockOn = false;
                cameraHandler.ClearLockOnTargets();
            }

            if (flag_LockOn && input_Right_Stick_Left)
            {
                input_Right_Stick_Left = false;
                cameraHandler.HandleLockOn();

                if(cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (flag_LockOn && input_Right_Stick_Right)
            {
                input_Right_Stick_Right = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }
        }

        private void HandleTwoHandInput()
        {
            if(input_Y)
            {
                input_Y = false;

                flag_TwoHand = !flag_TwoHand;

                if(flag_TwoHand)
                {
                    // Enable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }

                else
                {
                    // Disable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }
        }

        private void HandleCriticalAttackInput()
        {
            if(input_CriticalAttack && !playerManager.isInteracting)
            {
                input_CriticalAttack = false;
                playerAttacker.AttemptBackStabOrParry();
            }
        }
    }
}
