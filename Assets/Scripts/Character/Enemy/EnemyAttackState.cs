using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyAttackState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimator)
        {
            // Select one of the enemy's attacks based on its attacks score
            // If the selected attack is not able to be used because of bad angle or distance, select a new attack
            // If the attack is viable, stop our movement and attack our target
            // Set  the recovery timer to the attacks recovery timer 
            // Return the combat stance state
            return this;
        }
    }
}

