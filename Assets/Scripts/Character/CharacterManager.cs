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

        private void Awake()
        {

        }
    }
}
