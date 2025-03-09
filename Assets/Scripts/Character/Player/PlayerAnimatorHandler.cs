using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerAnimatorHandler : CharacterAnimatorHandler
    {
        private PlayerManager playerManager;
        private PlayerStats playerStats;
        private InputManager inputManager;
        private PlayerLocomotion playerLocomotion;
        private RagdollController ragdollController;

        private int vertical;
        private int horizontal;

        public void Initialise()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            animator = GetComponent<Animator>();
            inputManager = GetComponentInParent<InputManager>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            ragdollController = GetComponentInParent<RagdollController>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float _v = 0;

            if(verticalMovement > 0 && verticalMovement < 0.55f)
                _v = 0.5f;

            else if (verticalMovement > 0.55f)
                _v = 1;

            else if (verticalMovement < 0 && verticalMovement > -0.55f)
                _v = 0.5f;

            else if (verticalMovement < -0.55f)
                _v = -1;

            else
                _v = 0;
            #endregion

            #region Horizontal
            float _h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
                _h = 0.5f;

            else if (horizontalMovement > 0.55f)
                _h = 1;

            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
                _h = 0.5f;

            else if (horizontalMovement < -0.55f)
                _h = -1;

            else
                _h = 0;
            #endregion

            if(isSprinting)
            {
                _v = 2;
                _h = horizontalMovement;
            }

            animator.SetFloat(vertical, _v, 0.1f, Time.deltaTime);
            animator.SetFloat(horizontal, _h, 0.1f, Time.deltaTime);
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
            playerManager.isParrying = true;
        }

        public void DisableIsParrying()
        {
            playerManager.isParrying = false;
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
            playerManager.canBeRiposted = true;
        }

        public void DisableCanBeRiposted()
        {
            playerManager.canBeRiposted = false;
        }


        public override void TakeCriticalDamageAnimationEvent()
        {
            // Dont play animation here, let the animator bool decide death animation for backstabs
            playerStats.TakeHealthDamageNoAnimation(playerManager.pendingCriticalDamage);
            playerManager.pendingCriticalDamage = 0;
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float _delta = Time.deltaTime;

            playerLocomotion.rigidbody.linearDamping = 0;

            Vector3 _deltaPosition = animator.deltaPosition;
            _deltaPosition.y = 0;

            Vector3 _velocity = _deltaPosition / _delta;
            playerLocomotion.rigidbody.linearVelocity = _velocity;
        }
    }
}