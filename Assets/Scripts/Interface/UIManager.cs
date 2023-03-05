using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;
        //private InventoryGold inventoryGold;
        //private PlayerStats playerStats;

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

        private void Awake()
        {
            
        }

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();

            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);

            Cursor.visible = false;
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            //change this for loop so it counts backwards if you want new items to show first
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if(i < playerInventory.weaponsInventory.Count)
                {
                    if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
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
            #endregion        
        }

        public void OpenLeftPanelWindow()
        {
            leftPanel.SetActive(true);
            Cursor.visible = true;
            //inventoryWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseLeftPanelWindows()
        {
            ResetAllSelectedSlots();
            leftPanel.SetActive(false);
            Cursor.visible = false;
            //equipmentWindow.SetActive(false);
            //inventoryWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }
    }
}

