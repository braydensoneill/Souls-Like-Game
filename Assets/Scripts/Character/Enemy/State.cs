using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public abstract class State : MonoBehaviour
    {
        //[Header("Enemy Actions")]
        //public EnemyAttackAction[] enemyAttacks;
        //public EnemyAttackAction currentAttack;

        public abstract State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimator);
    }
}

