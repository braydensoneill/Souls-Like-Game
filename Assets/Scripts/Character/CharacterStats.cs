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
        public float health_Current;

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
    }
}
