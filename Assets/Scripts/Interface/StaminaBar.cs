using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;

        public void SetBarMaxStamina(float maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }

        public void SetBarCurrentStamina(float currentValue)
        {
            slider.value = currentValue;
        }
    }
}

