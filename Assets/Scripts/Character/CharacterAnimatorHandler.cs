using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CharacterAnimatorHandler : MonoBehaviour
    {
        public Animator animator;
        public bool canRotate;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", false);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {

        }

        public void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }

        public void StopRotation()
        {
            animator.SetBool("canRotate", false); ;
        }
    }
}