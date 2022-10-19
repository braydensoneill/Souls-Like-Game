using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Health Stats")]
        public float health_Level = 10;
        public float health_Max;
        public float health_Current;

        [Header("Stamina Stats")]
        public float stamina_Level = 10;
        public float stamina_Max;
        public float stamina_Current;
    }
}
