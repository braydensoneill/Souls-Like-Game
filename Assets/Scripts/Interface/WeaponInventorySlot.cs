using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BON
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        private PlayerInventory playerInventory;
        private PlayerWeaponSlotManager weaponSlotManager;
        private UIManager uiManager;

        private WeaponItem item;

        public bool itemIsEquippedInLeftHand;
        public bool itemIsEquippedInRightHand;

        public Image itemIcon;
        public TextMeshProUGUI itemName;

        public GameObject itemIsEquippedBackground;

        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            itemIcon.sprite = item.itemIcon;
            itemIcon.enabled = true;
            itemName.text = item.itemName;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            itemName.text = "null";
            gameObject.SetActive(false);
        }

        public bool IsEquipped()
        {
            return itemIsEquippedInLeftHand || itemIsEquippedInRightHand;
        }

        public WeaponItem GetItem()
        {
            return item;
        }

        // The method called by the Button's OnClick
        public void OnButtonClick()
        {
            // Check if item is already equipped
            if (IsEquipped())
                UnequipThisItem();
            else
                EquipThisItem();

            UpdateWeapons();
        }

        private void UpdateWeapons()
        {
            // Refresh the weapons after equipping or unequipping
            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];

            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

            uiManager.HighlightEquippedInventorySlots();
        }

        public void EquipThisItem()
        {
            if (item.isTwoHand)
            {
                // Equip to the right hand and unequip the left hand (two-handed weapon)
                EquipToRightHand();
                UnequipFromLeftHand();
            }
            else if (item.isOffHand)
            {
                // Equip to the left hand (off-hand weapon)
                EquipToLeftHand();
            }
            else
            {
                // Equip to the right hand (main-hand weapon)
                EquipToRightHand();
            }

            itemIsEquippedBackground.SetActive(true);
        }

        private void EquipToRightHand()
        {
            playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex] = item;
            itemIsEquippedInRightHand = true;
        }

        private void EquipToLeftHand()
        {
            playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex] = item;
            itemIsEquippedInLeftHand = true;
        }

        public void UnequipThisItem()
        {
            if (itemIsEquippedInRightHand)
            {
                UnequipFromRightHand();
            }
            else if (itemIsEquippedInLeftHand)
            {
                UnequipFromLeftHand();
            }

            itemIsEquippedBackground.SetActive(false);
        }

        private void UnequipFromRightHand()
        {
            playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex] = playerInventory.unarmedWeapon;
            itemIsEquippedInRightHand = false;
        }

        private void UnequipFromLeftHand()
        {
            playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex] = playerInventory.unarmedWeapon;
            itemIsEquippedInLeftHand = false;
        }
    }
}
