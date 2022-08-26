using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyStats : MonoBehaviour
    {
        public float healthLevel = 10;
        public float maxHealth;
        public float currentHealth;

        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            maxHealth = CalculateMaxHealth();
            currentHealth = maxHealth;
        }

        private float CalculateMaxHealth()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            animator.Play("Damage_01");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Dead_01");
            }
        }
    }
}