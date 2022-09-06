using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        public Image icon;
        private WeaponItem weaponItem;

        public void AddItem(WeaponItem newItem)
        {
            weaponItem = newItem;
            icon.sprite = weaponItem.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            weaponItem = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }
    }
}

