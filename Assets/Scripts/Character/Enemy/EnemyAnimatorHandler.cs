using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyAnimatorHandler : CharacterAnimatorHandler
    {
        private EnemyLocomotion enemyLocomotion;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyLocomotion = GetComponentInParent<EnemyLocomotion>();
        }

        /* Any time the animator plays an animation with roo motion, recenter the model
         * back on to the GameObject */
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyLocomotion.enemyRigidbody.drag = 0;

            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;

            Vector3 velocity = deltaPosition / delta;
            enemyLocomotion.enemyRigidbody.velocity = velocity;
        }
    }
}
