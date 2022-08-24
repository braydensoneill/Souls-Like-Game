using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator anim;
        private int vertical;
        private int horizontal;
        public bool canRotate;

        public void Initialise()
        {
            anim = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            #region Vertical
            float v = 0;

            if(verticalMovement > 0 && verticalMovement < 0.55f)
                v = 0.5f;

            else if (verticalMovement > 0.55f)
                v = 1;

            else if (verticalMovement < 0 && verticalMovement > -0.55f)
                v = 0.5f;

            else if (verticalMovement < -0.55f)
                v = -1;

            else
                v = 0;
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
                h = 0.5f;

            else if (horizontalMovement > 0.55f)
                h = 1;

            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
                h = 0.5f;

            else if (horizontalMovement < -0.55f)
                h = -1;

            else
                h = 0;
            #endregion

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }
    }
}