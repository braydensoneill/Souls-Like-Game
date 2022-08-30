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
        public string Idle_Arm_Right_01;
        public string Idle_Arm_Left_01;

        [Header("Attack Animations")]
        public string OH_Right_Sword_Attack_01;
        public string OH_Right_Sword_Attack_02;
        public string OH_Heavy_Attack_01;

        [Header("Stamina Costs")]
        public int baseStamina;
        public int lightAttackMultiplier;
        public int heavyAttackMultiplier;
    }
}
