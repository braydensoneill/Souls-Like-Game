using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyStats : MonoBehaviour
    {
        private Animator animator;

        [Header("Stats")]
        public float health_Level = 10;
        public float health_Max;
        public float health_Current;

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

        public void TakeDamage(int damage)
        {
            health_Current = health_Current - damage;

            animator.Play("Damage_01");

            if (health_Current <= 0)
            {
                health_Current = 0;
                animator.Play("Dead_01");
            }
        }
    }
}