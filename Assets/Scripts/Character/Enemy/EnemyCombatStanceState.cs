using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyCombatStanceState : State
    {
        public EnemyAttackState attackState;
        public EnemyPursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            
            // Potentially circle player or walk around them
            if(enemyManager.isInteracting)
            {
                enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }

            if (enemyManager.currentRecoveryTime <= 0 && 
                distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return attackState;
            }

            else if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return pursueTargetState;
            }

            else
            {
                return this;
            }
        }
    }
}

