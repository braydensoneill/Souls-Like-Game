using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerStats : MonoBehaviour
    {
        public float healthLevel = 10;
        public float maxHealth;
        public float currentHealth;

        public HealthBar healthBar;

        AnimatorHandler animatorHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            maxHealth = CalculateMaxHealth();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        private float CalculateMaxHealth()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Dead_01", true);
                //HandlePlayerDeath
            }
        }

    }
}

