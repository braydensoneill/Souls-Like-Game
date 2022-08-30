using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image rightWeaponIcon;
        public Image leftWeaponIcon;

        public void UpdateWeaponQuickslotsUI(bool isLeft, WeaponItem weaponItem)
        {
            if(isLeft)
            {
                if (weaponItem.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weaponItem.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }

            else
            {
                if(weaponItem.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weaponItem.itemIcon;
                    rightWeaponIcon.enabled = true;
                }

                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
        }
    }
}

