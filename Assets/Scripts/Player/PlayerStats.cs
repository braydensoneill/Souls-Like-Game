using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerStats : MonoBehaviour
    {
        public HealthBar health_Bar;
        public StaminaBar stamina_Bar;

        private AnimatorHandler animatorHandler;

        public float health_level = 10;
        public float health_max;
        public float health_current;

        public float stamina_level = 10;
        public float stamina_max;
        public float stamina_current;

        private void Awake()
        {
            health_Bar = FindObjectOfType<HealthBar>();
            stamina_Bar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            health_max = SetMaxHealthFromHealthLevel();
            health_current = health_max;
            health_Bar.SetBarMaxHealth(health_max);

            stamina_max = SetMaxStaminaFromStaminaLevel();
            stamina_current = stamina_max;
        }

        private float SetMaxHealthFromHealthLevel()
        {
            // Add whatever equation for calculating hp here
            health_max = health_level * 10;
            return health_max;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            // Add whatever equation for calculating hp here
            stamina_max = stamina_level * 10;
            return stamina_max;
        }

        public void TakeHealthDamage(int damage)
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

        public void TakeStaminaDamage(int damage)
        {
            stamina_current = stamina_current - damage;
            stamina_Bar.SetBarCurrentStamina(stamina_current);
        }

    }
}

