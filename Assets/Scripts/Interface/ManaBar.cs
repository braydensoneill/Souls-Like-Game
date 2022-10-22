using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class ManaBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxManaBarValue(float _maxValue)
        {
            slider.maxValue = _maxValue;
            slider.value = _maxValue;
        }

        public void SetCurrentManaBarValue(float _currentValue)
        {
            slider.value = _currentValue;
        }
    }
}