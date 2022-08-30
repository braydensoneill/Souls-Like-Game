using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;

        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightHandDamageCollider;

        private Animator animator;

        private QuickSlotsUI quickslotsUI;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            quickslotsUI = FindObjectOfType<QuickSlotsUI>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if(weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if(isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickslotsUI.UpdateWeaponQuickslotsUI(true, weaponItem);
                #region Handle Left Weapon Idle Animations
                if(weaponItem != null)
                    animator.CrossFade(weaponItem.Idle_Arm_Left_01, 0.2f);

                else
                    animator.CrossFade("Left_Arm_Empty", 0.2f);
                #endregion
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickslotsUI.UpdateWeaponQuickslotsUI(false, weaponItem);
                #region Handle Right Weapon Idle Animations
                if (weaponItem != null)
                    animator.CrossFade(weaponItem.Idle_Arm_Right_01, 0.2f);

                else
                    animator.CrossFade("Right_Arm_Empty", 0.2f);
                #endregion
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
    }
}
