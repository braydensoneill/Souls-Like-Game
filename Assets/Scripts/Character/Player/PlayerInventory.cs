using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerInventory : MonoBehaviour
    {
        private PlayerWeaponSlotManager weaponSlotManager;

        [Header("Weapon Slots")]
        public Spell currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public WeaponItem unarmedWeapon;

        public bool equipNextWeaponToRightHand;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

        public int currentRightWeaponIndex;
        public int currentLeftWeaponIndex;

        public List<WeaponItem> weaponsInventory;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<PlayerWeaponSlotManager>();
        }

        private void Start()
        {
            //rightWeapon = unarmedWeapon;
            //leftWeapon = unarmedWeapon;
        
            // Declare Weapons
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];

            // Load Weapons
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true); 

            equipNextWeaponToRightHand = true;
        }

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex += 1;

            if (currentRightWeaponIndex < weaponsInRightHandSlots.Length && weaponsInRightHandSlots[currentRightWeaponIndex] != null)
            {
                LoadRightWeapon(weaponsInRightHandSlots[currentRightWeaponIndex]);
            }
            else
            {
                LoadRightWeapon(unarmedWeapon);
                currentRightWeaponIndex = -1;
            }
        }

        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex += 1;

            if (currentLeftWeaponIndex < weaponsInLeftHandSlots.Length && weaponsInLeftHandSlots[currentLeftWeaponIndex] != null)
            {
                LoadLeftWeapon(weaponsInLeftHandSlots[currentLeftWeaponIndex]);
            }
            else
            {
                LoadLeftWeapon(unarmedWeapon);
                currentLeftWeaponIndex = -1;
            }
        }

        private void LoadRightWeapon(WeaponItem weapon)
        {
            rightWeapon = weapon;
            weaponSlotManager.LoadWeaponOnSlot(weapon, false);
        }

        private void LoadLeftWeapon(WeaponItem weapon)
        {
            leftWeapon = weapon;
            weaponSlotManager.LoadWeaponOnSlot(weapon, true);
        }
    }
}

