using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BON
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        private WeaponItem weaponItem;
        public Image itemIcon;
        public TextMeshProUGUI itemName;

        public void AddItem(WeaponItem newItem)
        {
            weaponItem = newItem;
            itemIcon.sprite = weaponItem.itemIcon;
            itemIcon.enabled = true;
            itemName.text = weaponItem.name;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            weaponItem = null;
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            itemName.text = "null";
            gameObject.SetActive(false);
        }
    }
}

