using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class DamageCollider : MonoBehaviour
    {
        public int CurrentWeaponDamage = 25;
        private Collider _damageCollider;

        private void Awake()
        {
            _damageCollider = GetComponent<Collider>();
            _damageCollider.gameObject.SetActive(true);
            _damageCollider.isTrigger = true;
            _damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            _damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            _damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                if(playerStats != null)
                {
                    playerStats.TakeDamage(CurrentWeaponDamage);
                }    
            }

            if (other.tag == "Enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(CurrentWeaponDamage);
                }
            }
        }
    }
}
