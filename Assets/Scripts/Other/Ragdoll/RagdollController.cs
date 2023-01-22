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
        }

        public void RagdollModeOn()
        {
            characterAnimator.enabled = false;

            foreach (Collider collider in allColliders)
                collider.enabled = false;

            foreach (Collider collider in ragdollLimbColliders)
                collider.enabled = true;

            foreach (Rigidbody rigidbody in ragdollLimbRigidbodies)
                rigidbody.isKinematic = false;

            characterCollider.enabled = false;
            characterRigidbody.isKinematic = true;
        }

        public void RagdollModeOff()
        {
            foreach(Collider collider in ragdollLimbColliders)
                collider.enabled = false;

            foreach (Rigidbody rigidbody in ragdollLimbRigidbodies)
                rigidbody.isKinematic = true;

            characterAnimator.enabled = true;
            characterCollider.enabled = true;
            characterRigidbody.isKinematic = false;
        }
    }
}