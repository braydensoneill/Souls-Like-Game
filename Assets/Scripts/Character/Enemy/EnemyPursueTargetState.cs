using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyPursueTargetState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimator)
        {
            // Chase the target
            // If within attack range, switch to combat stance state
            // If target is out of range, return this state and continue to chase the target
            return this;
        }
    }
}