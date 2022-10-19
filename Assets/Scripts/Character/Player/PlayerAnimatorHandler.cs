using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerAnimatorHandler : CharacterAnimatorHandler
    {
        private PlayerManager playerManager;
        private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;

        public bool canRotate;
        private int vertical;
        private int horizontal;

        public void Initialise()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();

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

        public void SetRotation(bool canRotate)
        {
            this.canRotate = canRotate;
        }

        public void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float _delta = Time.deltaTime;

            playerLocomotion.rigidbody.drag = 0;

            Vector3 _deltaPosition = animator.deltaPosition;
            _deltaPosition.y = 0;

            Vector3 _velocity = _deltaPosition / _delta;
            playerLocomotion.rigidbody.velocity = _velocity;
        }
    }
}