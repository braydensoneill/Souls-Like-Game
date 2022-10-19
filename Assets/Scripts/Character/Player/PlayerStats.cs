using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerStats : CharacterStats
    {
        public HealthBar health_Bar;
        public StaminaBar stamina_Bar;

        private PlayerAnimatorHandler animatorHandler;


        private void Awake()
        {
            health_Bar = FindObjectOfType<HealthBar>();
            stamina_Bar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            health_Max = SetMaxHealthFromHealthLevel();
            health_Current = health_Max;
            health_Bar.SetBarMaxHealth(health_Max);

            stamina_Max = SetMaxStaminaFromStaminaLevel();
            stamina_Current = stamina_Max;
        }

        private float SetMaxHealthFromHealthLevel()
        {
            // Add whatever equation for calculating hp here
            health_Max = health_Level * 10;
            return health_Max;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            // Add whatever equation for calculating hp here
            stamina_Max = stamina_Level * 10;
            return stamina_Max;
        }

        public void TakeHealthDamage(int damage)
        {
            health_Current = health_Current - damage;

            health_Bar.SetBarCurrentHealth(health_Current);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if(health_Current <= 0)
            {
                health_Current = 0;
                animatorHandler.PlayTargetAnimation("Dead_01", true);
                //HandlePlayerDeath
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            stamina_Current = stamina_Current - damage;
            stamina_Bar.SetBarCurrentStamina(stamina_Current);
        }

    }
}

