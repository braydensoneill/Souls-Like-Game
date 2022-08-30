﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if(playerStats != null)
            {
                playerStats.TakeHealthDamage(damage);
            }
        }
    }
}


