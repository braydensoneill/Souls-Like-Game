using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponItem attackingWeapon;

        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        // TBD leftHandSheathSlot
        // TBD rightHandSheathSlot
        public WeaponHolderSlot backSlot;

        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightHandDamageCollider;

        private Animator animator;

        private QuickSlotsUI quickslotsUI;

        private PlayerStats playerStats;
        InputHandler inputHandler;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            quickslotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickslotsUI.UpdateWeaponQuickslotsUI(true, weaponItem);
                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                    animator.CrossFade(weaponItem.Idle_Arm_Left_01, 0.2f);

                else
                    animator.CrossFade("Left_Arm_Empty", 0.2f);
                #endregion
            }
            else
            {
                if(inputHandler.flag_TwoHand)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon); // Move current left hand weapon to the back or disable it
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.Idle_TH, 0.2f);
                }
                else
                {
                    #region Handle Right Weapon Idle Animations

                    animator.CrossFade("Both Arms Empty", 0.2f);

                    backSlot.UnloadWeaponAndDestroy();

                    if (weaponItem != null)
                        animator.CrossFade(weaponItem.Idle_Arm_Right_01, 0.2f);

                    else
                        animator.CrossFade("Right_Arm_Empty", 0.2f);
                    #endregion
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickslotsUI.UpdateWeaponQuickslotsUI(false, weaponItem);
            }
        }

        #region Handle Weapon's Damage Collider
        public void LoadLeftWeaponDamageCollider()
        {
            _leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void LoadRightWeaponDamageCollider()
        {
            _rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenLeftDamageCollider()
        {
            _leftHandDamageCollider.EnableDamageCollider();
        }

        public void OpenRightDamageCollider()
        {
            _rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            _leftHandDamageCollider.DisableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            _rightHandDamageCollider.DisableDamageCollider();
        }
        #endregion

        #region Handle Weapon's Stamina Draining
        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion
    }
}
