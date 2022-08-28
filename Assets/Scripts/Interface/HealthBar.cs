using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class HealthBar : MonoBehaviour
    {
        public Slider _slider;

        public void SetBarMaxHealth(float maxHealth)
        {
            _slider.maxValue = maxHealth;
            _slider.value = maxHealth;
        }

        public void SetBarCurrentHealth(float currentHealth)
        {
            _slider.value = currentHealth;
        }
    }
}