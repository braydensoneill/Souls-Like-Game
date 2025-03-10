using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CharacterManager : MonoBehaviour
    {
        protected CharacterAnimatorHandler characterAnimatorHandler;
        protected CharacterStats characterStats;
        protected RagdollController ragdollController;

        [Header("Lock On")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider parryCollider;

        [Header("Combat Flags")]
        public bool canBeParried;
        public bool canBeRiposted;
        public bool isParrying;

        // Damage will be inflicted during an animation event
        // Used in backstab or ripsote animation
        [Header("Damage Multipliers")]
        public int pendingCriticalDamage;

        protected virtual void Awake()
        {
            characterAnimatorHandler = GetComponentInChildren<CharacterAnimatorHandler>();
            characterStats = GetComponent<CharacterStats>();
            ragdollController = GetComponentInParent<RagdollController>();
        }

        public virtual void HandleDeathState()
        {
            // Check if the enemy is already dead and ragdoll mode is already on
            if (characterStats.getHealthCurrent() > 0 || ragdollController.IsRagdolling())
                return;

            // Set health to zero (just in case) and mark the enemy as dead
            characterStats.setHealthCurrent(0);
            characterStats.isDead = true;

            // Handle death-related actions (like tagging)
            this.gameObject.tag = "Untagged";

            // Disable the Animator to cancel any ongoing animations
            if (characterAnimatorHandler.animator != null)
            {
                characterAnimatorHandler.animator.enabled = false;
            }

            // Only call ragdoll mode once
            ragdollController.RagdollModeOn();
        }
    }
}
