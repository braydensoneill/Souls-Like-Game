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

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;

        [Header("Attack Animations")]
        public string oh_right_sword_attack_01;
        public string oh_right_sword_attack_02;
        public string oh_heavy_attack_01;
    }
}
