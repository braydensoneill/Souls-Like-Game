using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetCurrentHealth(float currentHealth)
        {
            slider.value = currentHealth;
        }
    }
}