using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class RagdollController : MonoBehaviour
    {
        // reference to the character's rigidbody
        private Rigidbody characterRigidbody;

        // reference to the character's collider
        [SerializeField] CapsuleCollider characterCollider;

        // reference to the character's combat collision blocker collider
        [SerializeField] CapsuleCollider characterCollisionBlocker;

        // reference to the character's characterStats
        private CharacterStats characterStats;

        // list of all the colliders in the ragdoll
        private List<Collider> ragdollColliders;

        void Awake()
        {
            // get the character's rigidbody
            characterRigidbody = GetComponent<Rigidbody>();

            // get the character's characterStats
            characterStats = GetComponent<CharacterStats>();

            // get all the colliders in the ragdoll
            ragdollColliders = new List<Collider>(GetComponentsInChildren<Collider>());
        }

        public void StartRagdoll()
        {
            // disable the character's rigidbody
            characterRigidbody.isKinematic = true;

            // disable the character's collider
            characterCollider.isTrigger = true;

            // disable the character's collision blocker collider
            characterCollisionBlocker.isTrigger = true;

            // enable all the ragdoll colliders
            foreach (Collider collider in ragdollColliders)
                collider.enabled = true;
        }
    }
}