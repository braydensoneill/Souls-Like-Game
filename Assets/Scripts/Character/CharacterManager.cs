using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider parryCollider;

        [Header("Combat Flags")]
        public bool canBeParried;
        public bool canBeRipsoted;
        public bool isParrying;

        // Damage will be inflicted during an animation event
        // Used in backstab or ripsote animation
        [Header("Damage Multipliers")]
        public int pendingCriticalDamage;
    }
}
