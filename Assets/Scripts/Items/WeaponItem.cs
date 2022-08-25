using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("One Handed Attacks Animations")]
        public string OH_Light_Attack_1;
        public string OH_Heavy_Attack_1;
    }
}
