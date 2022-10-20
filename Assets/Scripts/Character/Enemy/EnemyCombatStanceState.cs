using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyCombatStanceState : State
    {
        public EnemyAttackState attackState;
        public EnemyPursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimator)
        {
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            
            // Potentially circle player or walk around them

            if (enemyManager.currentRecoveryTime <= 0 && 
                enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return attackState;
            }

            else if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return pursueTargetState;
            }

            else
            {
                return this;
            }

            return this;
        }
    }
}

