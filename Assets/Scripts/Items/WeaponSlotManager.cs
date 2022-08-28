using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();

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
                #region Handle Left Weapon Idle Animations
                if(weaponItem != null)
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);

                else
                    animator.CrossFade("Left Arm Empty", 0.2f);
                #endregion
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                #region Handle Right Weapon Idle Animations
                if (weaponItem != null)
                    animator.CrossFade(weaponItem.right_hand_idle, 0.2f);

                else
                    animator.CrossFade("Right Arm Empty", 0.2f);
                #endregion
            }
        }

        #region Handle Weapon's Damage Collider
        public void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        #endregion
    }

}
