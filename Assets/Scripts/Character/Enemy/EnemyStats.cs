using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyStats : CharacterStats
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            health_Max = CalculateMaxHealth();
            health_Current = health_Max;
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

            animator.Play("Damage_01");

            if (health_Current <= 0)
            {
                health_Current = 0;
                animator.Play("Dead_01");
                isDead = true;
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
    }
}