using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxHealthBarValue(float _maxValue)
        {
            slider.maxValue = _maxValue;
            slider.value = _maxValue;
        }

        public void SetCurrentHealthBarValue(float _currentValue)
        {
            slider.value = _currentValue;
        }
    }
}