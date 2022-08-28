using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerStats : MonoBehaviour
    {
        public float health_level = 10;
        public float health_max;
        public float health_current;

        public HealthBar health_Bar;

        AnimatorHandler animatorHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            health_max = CalculateMaxHealth();
            health_current = health_max;
            health_Bar.SetBarMaxHealth(health_max);
        }

        private float CalculateMaxHealth()
        {
            health_max = health_level * 10;
            return health_max;
        }

        public void TakeDamage(int damage)
        {
            health_current = health_current - damage;

            health_Bar.SetBarCurrentHealth(health_current);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if(health_current <= 0)
            {
                health_current = 0;
                animatorHandler.PlayTargetAnimation("Dead_01", true);
                //HandlePlayerDeath
            }
        }

    }
}

