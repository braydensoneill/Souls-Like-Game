using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        public int CurrentWeaponDamage = 25;
        private Collider damageCollider;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider _other)
        {
            // do not allow a character to take damage from their own weapon
            if (gameObject.transform.IsChildOf(_other.gameObject.transform))
                return;

            if(_other.tag == "Player")
            {
                PlayerStats playerStats = _other.GetComponent<PlayerStats>();
                CharacterManager playerCharacterManager = _other.GetComponent<CharacterManager>();

                if(playerCharacterManager != null)
                {
                    if(playerCharacterManager.isParrying)
                    {
                        // Check here if you are parryable
                        characterManager.GetComponentInChildren<CharacterAnimatorHandler>().PlayTargetAnimation("Parry_Parried", true);
                        return;
                    }
                }

                if(playerStats != null)
                {
                    playerStats.TakeHealthDamage(CurrentWeaponDamage);
                }    
            }

            if (_other.tag == "Enemy")
            {
                EnemyStats enemyStats = _other.GetComponent<EnemyStats>();
                CharacterManager enemyCharacterManager = _other.GetComponent<CharacterManager>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        // Check here if you are parryable
                        characterManager.GetComponentInChildren<CharacterAnimatorHandler>().PlayTargetAnimation("Parry_Parried", true);
                        return;
                    }
                }

                if (enemyStats != null)
                {
                    enemyStats.TakeHealthDamage(CurrentWeaponDamage);
                }
            }
        }
    }
}
