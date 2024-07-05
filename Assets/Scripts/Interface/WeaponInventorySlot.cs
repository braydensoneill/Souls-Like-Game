using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

        public void SelectThisItem(int _mouseButtonPressed)
        {
            if (IsEquipped())
                UnequipThisItem();
            else
                EquipThisItem(_mouseButtonPressed);

            UpdateWeapons();
            uiManager.HighlightEquippedInventorySlots();
        }

        private void UpdateWeapons()
        {
            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];

            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
        }

        public void EquipThisItem(int _mouseButtonPressed)
        {
            if (_mouseButtonPressed == 1)
            {
                // Clear previous right hand equipped status
                WeaponItem currentRightHandWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
                if (currentRightHandWeapon != null)
                {
                    uiManager.ClearEquippedStatusForItem(currentRightHandWeapon);
                }

                EquipToHand(ref playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex], ref itemIsEquippedInRightHand, false);
            }
            
            else if (_mouseButtonPressed == 2)
            {
                // Clear previous left hand equipped status
                WeaponItem currentLeftHandWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];
                if (currentLeftHandWeapon != null)
                {
                    uiManager.ClearEquippedStatusForItem(currentLeftHandWeapon);
                }

                EquipToHand(ref playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex], ref itemIsEquippedInLeftHand, true);
            }

            // Set the equipped background
            itemIsEquippedBackground.SetActive(true);
        }

        private void EquipToHand(ref WeaponItem handSlot, ref bool isEquippedFlag, bool equipNextToRight)
        {
            handSlot = item;
            isEquippedFlag = true;
            playerInventory.equipNextWeaponToRightHand = equipNextToRight;
        }

        public void UnequipThisItem()
        {
            if (itemIsEquippedInRightHand)
            {
                UnequipFromHand(ref playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex], ref itemIsEquippedInRightHand, true);
            }
            else if (itemIsEquippedInLeftHand)
            {
                UnequipFromHand(ref playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex], ref itemIsEquippedInLeftHand, false);
            }

            // Clear the equipped background
            itemIsEquippedBackground.SetActive(false);
        }

        private void UnequipFromHand(ref WeaponItem handSlot, ref bool isEquippedFlag, bool equipNextToRight)
        {
            handSlot = playerInventory.unarmedWeapon;
            isEquippedFlag = false;
            playerInventory.equipNextWeaponToRightHand = equipNextToRight || playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex] == playerInventory.unarmedWeapon;
        }
    }
}
