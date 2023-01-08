using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CharacterAnimatorHandler : MonoBehaviour
    {
        public Animator animator;
        public bool canRotate;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", canRotate);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }

        public void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }

        public void StopRotation()
        {
            animator.SetBool("canRotate", false); ;
        }

        public void DisableAnimator()
        {
            animator.applyRootMotion = false; // this was meant for ragdoll system, it doesnt work, dont use this yet
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {

        }

        public virtual void StartRagdoll()
        {

        }
    }
}