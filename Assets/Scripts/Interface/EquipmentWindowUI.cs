using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        public HandEquipmentSlotUI[] handEquipmentSlotUI;

        private void Start()
        {

        }

        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            /*
             * If the handEquipmentSlot is rightHandSlot01 then add the item from the 
             * rightHandSlot at position 0 from the player's inventory. Do this for all
             * slots using this for loop
             */

            for(int i = 0; i < handEquipmentSlotUI.Length; i++)
            {
                if (handEquipmentSlotUI[i].rightHandSlot01)
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);

                else if (handEquipmentSlotUI[i].leftHandSlot01)
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);


                /*I have decided to remove multiple weapon slot functionality for now
                Every thing will be done the same way, just as an array size of 1 in case I change my mind
                in the future
                 */
                //else if (handEquipmentSlotUI[i].rightHandSlot02)
                //handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);

                //else if (handEquipmentSlotUI[i].leftHandSlot02)
                    //handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
            }
        }

        public void SelectRightHandSlot01()
        {
            rightHandSlot01Selected = true;
        }

        public void SelectRightHandSlot02()
        {
            rightHandSlot02Selected = true;
        }

        // maybe add functionality for 2 more right hand slots

        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }

        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }

        // maybe add functionality for 2 more left hand slots

    }
}

