using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class RagdollController : MonoBehaviour
    {
        // character component references
        private Animator characterAnimator;
        public GameObject characterGameObject;
        public GameObject characterSprite;
        private Rigidbody characterRigidbody;
        private Collider characterCollider;

        // character limb component references
        private Collider[] allColliders;

        private Collider[] ragdollLimbColliders;
        private Rigidbody[] ragdollLimbRigidbodies;

        // ragdoll state
        private bool isRagdolling;


        void Awake()
        {
            // get the character's components
            characterAnimator = GetComponentInChildren<Animator>();
            characterRigidbody = GetComponent<Rigidbody>();
            characterCollider = GetComponent<Collider>();
            allColliders = characterGameObject.GetComponentsInChildren<Collider>();

            // get the character's limbs' components
            ragdollLimbColliders = characterSprite.GetComponentsInChildren<Collider>();
            ragdollLimbRigidbodies = characterSprite.GetComponentsInChildren<Rigidbody>();

            isRagdolling = false;

            RagdollModeOff();
        }

        public void RagdollModeOn()
        {
            characterAnimator.enabled = false;

            // Disable the original character collider to avoid conflicts with ragdoll physics
            characterCollider.enabled = false;

            // Disable all character colliders
            foreach (Collider collider in allColliders)
                collider.enabled = false;

            // Enable the ragdoll limb colliders
            foreach (Collider collider in ragdollLimbColliders)
                collider.enabled = true;

            // Disable kinematic mode on ragdoll rigidbodies to allow physics interaction
            foreach (Rigidbody rigidbody in ragdollLimbRigidbodies)
                rigidbody.isKinematic = false;

            // Make the main character rigidbody kinematic to prevent it from interacting with physics
            characterRigidbody.isKinematic = true;
        }

        public void RagdollModeOff()
        {
            // Disable ragdoll colliders
            foreach(Collider collider in ragdollLimbColliders)
                collider.enabled = false;

            // Enable kinematic mode on ragdoll rigidbodies
            foreach (Rigidbody rigidbody in ragdollLimbRigidbodies)
                rigidbody.isKinematic = true;

            // Enable the character's collider
            characterCollider.enabled = true;

            // Re-enable the character animator for normal behavior
            characterAnimator.enabled = true;

            // Make the character's rigidbody non-kinematic
            characterRigidbody.isKinematic = false;
        }

        public bool IsRagdolling() {
            return isRagdolling;
        }
    }
}