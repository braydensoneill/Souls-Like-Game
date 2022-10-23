using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class DamageCollider : MonoBehaviour
    {
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

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                if(playerStats != null)
                {
                    playerStats.TakeHealthDamage(CurrentWeaponDamage);
                }    
            }

            if (other.tag == "Enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();

                if (enemyStats != null)
                {
                    enemyStats.TakeHealthDamage(CurrentWeaponDamage);
                }
            }
        }
    }
}
