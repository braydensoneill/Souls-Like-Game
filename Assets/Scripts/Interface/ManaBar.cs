using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class ManaBar : MonoBehaviour
    {
        public Slider slider;

        public void SetBarMaxMana(float maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }

        public void SetBarCurrentMana(float currentValue)
        {
            slider.value = currentValue;
        }
    }
}