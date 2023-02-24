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

        [Header("Player Health Regeneration")]
        public float health_Regeneration_Strength = 2;
        [SerializeField] private float health_Regeneration_Timer_Current = 0;
        [SerializeField] private float health_Regeneration_Timer_Max = 1;

        [Header("Player Stamina Regeneration")]
        public float stamina_Regeneration_Strength = 10;
        [SerializeField] private float stamina_Regeneration_Timer_Current = 0;
        [SerializeField] private float stamina_Regeneration_Timer_Max = 1;

        [Header("Player Mana Regeneration")]
        public float mana_Regeneration_Strength = 10;
        [SerializeField] private float mana_Regeneration_Timer_Current = 0;
        [SerializeField] private float mana_Regeneration_Timer_Max = 1;

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
            health_Bar.SetMaxBarValue(health_Max);
            //health_Bar.SetCurrentBarValue(health_Current);

            // Initialise Stamina Values
            stamina_Max = SetMaxStaminaFromStaminaLevel();
            stamina_Current = stamina_Max;
            stamina_Bar.SetMaxBarValue(stamina_Max);
            //stamina_Bar.SetCurrentBarValue(stamina_Current);

            // Initialise Mana Values
            mana_Max = SetMaxManaFromManaLevel();
            mana_Current = mana_Max;
            mana_Bar.SetMaxBarValue(mana_Max);
            //mana_Bar.SetCurrentBarValue(mana_Current);
        }

        private void Update()
        {
            //health_Bar.SetCurrentBarValue(health_Current);
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

            health_Current -= _amount;

            health_Bar.SetCurrentBarValue(health_Current);
            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if(health_Current <= 0)
            {
                health_Current = 0;
                animatorHandler.PlayTargetAnimation("Dead_01", true);
                isDead = true;
            }
        }

        public void TakeHealthDamageNoAnimation(int _amount)
        {
            if (isDead)
                return;

            health_Current = health_Current - _amount;
            health_Bar.SetCurrentBarValue(health_Current);

            if (health_Current <= 0)
            {
                health_Current = 0;
                isDead = true;
            }
        }

        public void TakeStaminaDamage(float _amount)
        {
            stamina_Current -= _amount;
            stamina_Bar.SetCurrentBarValue(stamina_Current);

            if (stamina_Current <= 0)
                stamina_Current = 0;
        }

        public void TakeManaDamage(float _amount)
        {
            mana_Current -= _amount;
            mana_Bar.SetCurrentBarValue(mana_Current);

            if(mana_Current <= 0)
                mana_Current = 0;
        }

        public void PassiveRegenerateHealth()
        {
            if (playerManager.isInteracting)
            {
                health_Regeneration_Timer_Current = 0;
                return;
            }
                
            health_Regeneration_Timer_Current += Time.deltaTime;

            if (health_Current < health_Max &&
                health_Regeneration_Timer_Current > health_Regeneration_Timer_Max)
            {
                RestoreHealth(health_Regeneration_Strength * Time.deltaTime);
            }            
        }

        public void PassiveRegenerateStamina()
        {
            if(playerManager.isInteracting)
            {
                stamina_Regeneration_Timer_Current = 0;
                return;
            }

            stamina_Regeneration_Timer_Current += Time.deltaTime;

            if (stamina_Current < stamina_Max &&
                stamina_Regeneration_Timer_Current > stamina_Regeneration_Timer_Max)
            {
                RestoreStamina(stamina_Regeneration_Strength * Time.deltaTime);
            }
        }

        public void PassiveRegenerateMana()
        {
            if (playerManager.isInteracting)
            {
                mana_Regeneration_Timer_Current = 0;
                return;
            }

            mana_Regeneration_Timer_Current += Time.deltaTime;

            if (mana_Current < mana_Max &&
                mana_Regeneration_Timer_Current > mana_Regeneration_Timer_Max)
            {
                RestoreMana(mana_Regeneration_Strength * Time.deltaTime);
            }
        }

        public void RestoreHealth(float _amount)
        {
            health_Current += _amount;
            health_Bar.SetCurrentBarValue(health_Current);

            if (health_Current > health_Max)
                health_Current = health_Max;
        }

        public void RestoreStamina(float _amount)
        {
            stamina_Current += _amount;
            stamina_Bar.SetCurrentBarValue(stamina_Current);

            if (stamina_Current > stamina_Max)
                stamina_Current = stamina_Max;
        }

        public void RestoreMana(float _amount)
        {
            mana_Current += _amount;
            mana_Bar.SetCurrentBarValue(mana_Current);

            if (mana_Current > mana_Max)
                mana_Current = mana_Max;
        }


        public void AddGold(int _gold)
        {
            gold_Current = gold_Current + _gold;
        }

    }
}

