using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;

        private WeaponHolderSlot rightHandSlot;
        private WeaponHolderSlot leftHandSlot;

        private DamageCollider leftHandDamageCollider;
        private DamageCollider rightHandDamageCollider;

        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                    leftHandSlot = weaponSlot;

                else if (weaponSlot.isRightHandSlot)
                    rightHandSlot = weaponSlot;
            }
        }

        private void Start()
        {
            LoadWeaponsOnBothHands();
        }

        public void LoadWeaponSlot(WeaponItem _weapon, bool _isLeft)
        {
            if (_isLeft)
            {
                leftHandSlot.currentWeapon = _weapon;
                leftHandSlot.LoadWeaponModel(_weapon);
                LoadWeaponDamageColldier(true); // true means left hand
            }

            else
            {
                rightHandSlot.currentWeapon = _weapon;
                rightHandSlot.LoadWeaponModel(_weapon);
                LoadWeaponDamageColldier(false);    // false means right hand
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponSlot(rightHandWeapon, false); // False means right hand
            }

            if (leftHandWeapon != null)
            {
                LoadWeaponSlot(leftHandWeapon, true);   // True means left hand
            }
        }

        public void LoadWeaponDamageColldier(bool _isLeft)
        {
            if (_isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            }

            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            }
        }

        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        #region Handle Weapon's Stamina Draining
        public void DrainStaminaLightAttack()
        {

        }

        public void DrainStaminaHeavyAttack()
        {

        }
        #endregion

        #region Handle Combo
        public void EnableCombo()
        {

        }

        public void DisableCombo()
        {

        }
        #endregion
    }

}
