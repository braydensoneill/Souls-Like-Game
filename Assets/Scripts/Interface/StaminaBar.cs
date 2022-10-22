using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxStaminaBarValue(float _maxValue)
        {
            slider.maxValue = _maxValue;
            slider.value = _maxValue;
        }

        public void SetCurrentStaminaBarValue(float _currentValue)
        {
            slider.value = _currentValue;
        }
    }
}

