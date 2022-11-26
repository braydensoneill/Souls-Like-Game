using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyStats : CharacterStats
    {
        private EnemyAnimatorHandler enemyAnimatorHandler;

        private void Awake()
        {
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        }

        void Start()
        {
            health_Max = CalculateMaxHealth();
            health_Current = health_Max;
            gold_Current = Random.Range(1, 20);
        }

        private float CalculateMaxHealth()
        {
            health_Max = health_Level * 10;
            return health_Max;
        }

        public void TakeHealthDamage(int _amount)
        {
            if (isDead)
                return;

            health_Current = health_Current - _amount;

            enemyAnimatorHandler.PlayTargetAnimation("Damage_01", true);

            if (health_Current <= 0)
            {
                HandleDeath();
            }
        }

        public void TakeHealthDamageNoAnimation(int _amount)
        {
            if (isDead)
                return;

            health_Current = health_Current - _amount;

            if (health_Current <= 0)
            {
                health_Current = 0;
                isDead = true;
            }
        }

        public void HandleDeath()
        {
            health_Current = 0;
            enemyAnimatorHandler.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }
}