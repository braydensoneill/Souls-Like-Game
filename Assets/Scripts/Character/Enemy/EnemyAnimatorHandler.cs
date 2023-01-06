using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyAnimatorHandler : CharacterAnimatorHandler
    {
        private EnemyManager enemyManager;
        private EnemyStats enemyStats;
        private RagdollController ragdollController;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
            ragdollController = GetComponentInParent<RagdollController>();
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            // Dont play animation here, let the animator bool decide death animation for backstabs

            enemyStats.TakeHealthDamageNoAnimation(enemyManager.pendingCriticalDamage);
            enemyManager.pendingCriticalDamage = 0;
        }

        public void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        public void EnableIsParrying()
        {
            enemyManager.isParrying = true;
        }

        public void DisableIsParrying()
        {
            enemyManager.isParrying = false;
        }

        public void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }

        public void DisableIsInvulnderable()
        {
            animator.SetBool("isInvulnerable", false);
        }

        public void EnableCanBeRiposted()
        {
            enemyManager.canBeRiposted = true;
        }

        public void DisableCanBeRiposted()
        {
            enemyManager.canBeRiposted = false;
        }

        public void AwardGoldOnDeath()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            InventoryGold inventoryGold = FindObjectOfType<InventoryGold>();

            if (playerStats != null)
            {
                playerStats.AddGold(enemyStats.gold_Current);
            }

            // If adding multiplayer, scan for every player in the scene and award them the appropriate gold
        }

        public override void StartRagdoll()
        {
            ragdollController.StartRagdoll();
        }

        /* Any time the animator plays an animation with roo motion, recenter the model
         * back on to the GameObject */
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;

            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;

            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;
        }
    }
}
