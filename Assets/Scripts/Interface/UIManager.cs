using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject leftPanel;
        public GameObject equipmentWindow;
        public GameObject inventoryWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            Cursor.visible = false;
        }

        public void UpdateWeaponInventorySlots()
        {
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }

                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
        }

        public void OpenLeftPanelWindow()
        {
            leftPanel.SetActive(true);
            Cursor.visible = true;
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseLeftPanelWindows()
        {
            leftPanel.SetActive(false);
            Cursor.visible = false;
        }

        public void HighlightEquippedInventorySlots()
        {
            foreach (var slot in weaponInventorySlots)
            {
                slot.itemIsEquippedBackground.SetActive(slot.getIsEquipped());
            }
        }

        public void ClearEquippedStatusForItem(WeaponItem item)
        {
            foreach (WeaponInventorySlot slot in weaponInventorySlots)
            {
                if (slot.GetItem() == item)
                {
                    slot.itemIsEquippedInLeftHand = false;
                    slot.itemIsEquippedInRightHand = false;
                    slot.itemIsEquippedBackground.SetActive(false);
                }
            }
        }
    }
}
