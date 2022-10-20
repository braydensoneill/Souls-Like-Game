using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyCombatStanceState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimator)
        {
            // Check for attack range
            // Potentially circle player or walk around them
            // If in attack range, return attack state
            // If on cooldown after attacking, return this state and continue circling player
            // If the player runs out of range, return the PursueTarget state
            return this;
        }
    }
}

