using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        [Header("General")]
        public GameObject modelPrefab;
        public bool isUnarmed;
        public bool isHeavy;
        public bool isLight;
        public bool isBow;
        public bool isShield;

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Idle Animations")]
        public string Idle_Arm_Right_01;
        public string Idle_Arm_Left_01;
        public string Idle_TH;

        [Header("Attack Animations")]
        public string OH_Sword_Light_Attack_Right_01;
        public string OH_Sword_Light_Attack_Right_02;
        public string OH_Heavy_Attack_01;
        public string TH_Sword_Light_Attack_01;
        public string TH_Sword_Light_Attack_02;

        [Header("Stamina Costs")]
        public int baseStamina;
        public int lightAttackMultiplier;
        public int heavyAttackMultiplier;

        [Header("Weapon Type")]
        public bool isSpellCaster;
        public bool isFaithCaster;
        public bool isPyroCaster;
        public bool isMeleeWeapon;
    }
}
