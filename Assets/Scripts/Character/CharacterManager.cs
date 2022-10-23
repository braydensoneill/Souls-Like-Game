using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CharacterManager : MonoBehaviour
    {
        public BackStabCollider backStabCollider;

        [Header("Lock On")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public BoxCollider backStabBoxCollider;

        // Damage will be inflicted during an animation event
        // Used in backstab or ripsote animation
        [Header("Damage Multipliers")]
        public int pendingCriticalDamage;
    }
}
