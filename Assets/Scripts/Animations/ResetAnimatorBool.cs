using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string targetBool;
        public bool status;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(targetBool, status);
        }
    }
}

