using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerWeaponSlotManager : MonoBehaviour
    {
        private PlayerStats playerStats;
        private InputHandler inputHandler;
        private PlayerInventory playerInventory;
        private PlayerManager playerManager;

        public WeaponItem attackingWeapon;

        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;

        #region Create sheath WeaponSlot variables
        public WeaponHolderSlot sheathHeavy01;
        public WeaponHolderSlot sheathHeavy02;
        public WeaponHolderSlot sheathLightRight01;
        public WeaponHolderSlot sheathLightRight02;
        public WeaponHolderSlot sheathLightLeft01;
        public WeaponHolderSlot sheathLightLeft02;
        public WeaponHolderSlot sheathShield01;
        public WeaponHolderSlot sheathShield02;
        public WeaponHolderSlot sheathBow01;
        public WeaponHolderSlot sheathBow02;
        //public WeaponHolderSlot backSlot;
        #endregion

        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        private Animator animator;

        private QuickSlotsUI quickslotsUI;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            quickslotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerManager = GetComponentInParent<PlayerManager>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                    leftHandSlot = weaponSlot;

                else if (weaponSlot.isRightHandSlot)
                    rightHandSlot = weaponSlot;

                else if (weaponSlot.isSheathHeavy01)
                    sheathHeavy01 = weaponSlot;

                else if (weaponSlot.isSheathHeavy02)
                    sheathHeavy02 = weaponSlot;

                else if (weaponSlot.isSheathLightRight01)
                    sheathLightRight01 = weaponSlot;

                else if (weaponSlot.isSheathLightRight02)
                    sheathLightRight02 = weaponSlot;

                else if (weaponSlot.isSheathLightLeft01)
                    sheathLightLeft01 = weaponSlot;

                else if (weaponSlot.isSheathLightLeft02)
                    sheathLightLeft02 = weaponSlot;

                else if (weaponSlot.isSheathShield01)
                    sheathShield01 = weaponSlot;

                else if (weaponSlot.isSheathShield02)
                    sheathShield02 = weaponSlot;

                else if (weaponSlot.isSheathBow01)
                    sheathBow01 = weaponSlot;

                else if (weaponSlot.isSheathBow02)
                    sheathBow02 = weaponSlot;
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
                Debug.Log("Weapon Item: " + weaponItem.itemName);
                #region Two-Handed Functionality if active
                if (inputHandler.flag_TwoHand)
                {
                    SheathLeftWeapon();
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.Idle_TH, 0.2f);
                }
                else
                {
                    #region Handle Right Weapon Idle Animations

                    animator.CrossFade("Both Arms Empty", 0.2f);

                    DestroyCurrentlySheathedLeftWeapon();

                    if (weaponItem != null)
                        animator.CrossFade(weaponItem.Idle_Arm_Right_01, 0.2f);

                    else
                        animator.CrossFade("Right_Arm_Empty", 0.2f);
                    #endregion
                }
                #endregion
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickslotsUI.UpdateWeaponQuickslotsUI(false, weaponItem);
            }
        }

        #region Handle Weapon's Damage Collider
        public void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.CurrentWeaponDamage = playerInventory.leftWeapon.baseDamage;
        }

        public void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.CurrentWeaponDamage = playerInventory.rightWeapon.baseDamage;
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
                rightHandDamageCollider.EnableDamageCollider();

            else if (playerManager.isUsingLeftHand)
                leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            if (playerManager.isUsingRightHand)
                rightHandDamageCollider.DisableDamageCollider();

            else if (playerManager.isUsingLeftHand)
                leftHandDamageCollider.DisableDamageCollider();
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

        private void SheathLeftWeapon()
        {
            if (playerInventory.leftWeapon.isHeavy)
                sheathHeavy01.LoadWeaponModel(leftHandSlot.currentWeapon);

            else if (playerInventory.leftWeapon.isLight)
                sheathLightLeft01.LoadWeaponModel(leftHandSlot.currentWeapon);

            else if (playerInventory.leftWeapon.isBow)
                sheathBow01.LoadWeaponModel(leftHandSlot.currentWeapon);

            else if (playerInventory.leftWeapon.isShield)
                sheathShield01.LoadWeaponModel(leftHandSlot.currentWeapon);

            // This else is just to put the item somewhere to prevent crashing if item is missing tag
            else
                sheathShield01.LoadWeaponModel(leftHandSlot.currentWeapon);
        }
        private void DestroyCurrentlySheathedLeftWeapon()
        {
            if (playerInventory.leftWeapon.isHeavy)
                sheathHeavy01.UnloadWeaponAndDestroy();

            else if (playerInventory.leftWeapon.isLight)
                sheathLightLeft01.UnloadWeaponAndDestroy();

            else if (playerInventory.leftWeapon.isBow)
                sheathBow01.UnloadWeaponAndDestroy();

            else if (playerInventory.leftWeapon.isShield)
                sheathShield01.UnloadWeaponAndDestroy();

            else
                return;
        }
    }
}
