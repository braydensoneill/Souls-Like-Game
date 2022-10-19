using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        public void SetBarMaxHealth(float maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }

        public void SetBarCurrentHealth(float currentValue)
        {
            slider.value = currentValue;
        }
    }
}