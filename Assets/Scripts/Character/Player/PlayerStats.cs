using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerStats : CharacterStats
    {
        private PlayerManager playerManager;
        private PlayerAnimatorHandler animatorHandler;

        [Header("Resource Bars")]
        public HealthBar health_Bar;
        public StaminaBar stamina_Bar;
        public ManaBar mana_Bar;

        [Header("Player Stamina Regeneration")]
        public float stamina_Regeneration_Strength = 6f;
        [SerializeField] private float stamina_Regeneration_Timer_Current = 0;
        [SerializeField] private float stamina_Regeneration_Timer_Max = 1.5f;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            health_Bar = FindObjectOfType<HealthBar>();
            stamina_Bar = FindObjectOfType<StaminaBar>();
            mana_Bar = FindObjectOfType<ManaBar>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Initialise Health Values
            health_Max = SetMaxHealthFromHealthLevel();
            health_Current = health_Max;
            health_Bar.SetMaxHealthBarValue(health_Max);
            health_Bar.SetCurrentHealthBarValue(health_Current);

            // Initialise Stamina Values
            stamina_Max = SetMaxStaminaFromStaminaLevel();
            stamina_Current = stamina_Max;
            stamina_Bar.SetMaxStaminaBarValue(stamina_Max);
            stamina_Bar.SetCurrentStaminaBarValue(stamina_Current);

            // Initialise Mana Values
            mana_Max = SetMaxManaFromManaLevel();
            mana_Current = mana_Max;
            mana_Bar.SetMaxManaBarValue(mana_Max);
            mana_Bar.SetCurrentManaBarValue(mana_Current);
        }

        private void Update()
        {
            health_Bar.SetCurrentHealthBarValue(health_Current);
        }

        private float SetMaxHealthFromHealthLevel()
        {
            // Add whatever equation for calculating hp here
            health_Max = health_Level * 10;
                return health_Max;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            // Add whatever equation for calculating stamina here
            stamina_Max = stamina_Level * 10;
                return stamina_Max;
        }

        private float SetMaxManaFromManaLevel()
        {
            // Add whatever equation for calculating mana here
            mana_Max = mana_Level * 10;
                return mana_Max;
        }

        public void TakeHealthDamage(float _amount)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            health_Current -= - _amount;

            health_Bar.SetCurrentHealthBarValue(health_Current);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if(health_Current <= 0)
            {
                health_Current = 0;
                animatorHandler.PlayTargetAnimation("Dead_01", true);
                isDead = true;
            }
        }

        public void TakeStaminaDamage(float _amount)
        {
            stamina_Current -= _amount;
            stamina_Bar.SetCurrentStaminaBarValue(stamina_Current);

            if (stamina_Current <= 0)
                stamina_Current = 0;
        }

        public void TakeManaDamage(float _amount)
        {
            mana_Current -= _amount;
            mana_Bar.SetCurrentManaBarValue(mana_Current);

            if(mana_Current <= 0)
                mana_Current = 0;
        }

        public void RegenerateStamina()
        {
            if(playerManager.isInteracting)
            {
                stamina_Regeneration_Timer_Current = 0;
            }

            else
            {
                stamina_Regeneration_Timer_Current += Time.deltaTime;

                if (stamina_Current <= stamina_Max && 
                    stamina_Regeneration_Timer_Current > stamina_Regeneration_Timer_Max)
                {
                    stamina_Current += stamina_Regeneration_Strength * (Time.deltaTime * stamina_Regeneration_Strength);
                    stamina_Bar.SetCurrentStaminaBarValue(Mathf.RoundToInt(stamina_Current));
                }
            } 
        }

        public void HealPlayer(float _amount)
        {
            health_Current += _amount;

            if(health_Current > health_Max)
                health_Current = health_Max;
        }



    }
}

