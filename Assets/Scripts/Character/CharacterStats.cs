using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("General")]
        public bool isDead;

        [Header("Health Stats")]
        public float health_Level = 10;
        public float health_Max;
        [SerializeField] protected float health_Current;  // will be changing all to serializefield

        [Header("Stamina Stats")]
        public float stamina_Level = 10;
        public float stamina_Max;
        public float stamina_Current;

        [Header("Mana Stats")]
        public float mana_Level = 10;
        public float mana_Max;
        public float mana_Current;

        [Header("Currency")]
        public int gold_Current = 0;

        public float getHealthCurrent() {
            return health_Current;
        }

        public void setHealthCurrent(float health_Current) {
            this.health_Current = health_Current;
        }
    }
}
