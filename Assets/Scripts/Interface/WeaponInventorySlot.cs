using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BON
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        PlayerInventory playerInventory;
        WeaponSlotManager weaponSlotManager;
        UIManager uiManager;

        private WeaponItem item;
        public Image itemIcon;
        public TextMeshProUGUI itemName;

        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
        }
        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            itemIcon.sprite = item.itemIcon;
            itemIcon.enabled = true;
            itemName.text = item.name;
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

        public void EquipThisItem()
        {
            if(uiManager.rightHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[0]);   // Add current item to inventory
                playerInventory.weaponsInRightHandSlots[0] = item;  // Equip this new item
                playerInventory.weaponsInventory.Remove(item);  // Remove item
            }

            else if(uiManager.rightHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[1]);
                playerInventory.weaponsInRightHandSlots[1] = item;  // Equip this new item
                playerInventory.weaponsInventory.Remove(item);  // Remove item
            }

            else if(uiManager.leftHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[0]);
                playerInventory.weaponsInLeftHandSlots[0] = item;  // Equip this new item
                playerInventory.weaponsInventory.Remove(item);  // Remove item
            }

            else if (uiManager.leftHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[1]);
                playerInventory.weaponsInLeftHandSlots[1] = item;  // Equip this new item#
                playerInventory.weaponsInventory.Remove(item);  // Remove item
            }

            else
            {
                return;
            }

            // Update the weapon models
            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];

            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

            // Update icons for equipped weapons in the equipment screen
            uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            uiManager.ResetAllSelectedSlots();
        }
    }
}

