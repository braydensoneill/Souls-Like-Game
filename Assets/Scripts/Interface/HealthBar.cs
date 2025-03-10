﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class HealthBar : MonoBehaviour
    {
        public Transform cameraTransform;
        private Slider slider;

        private float hideBarTimerCurrent = 0;
        private float hideBarTimerMax = 3;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        private void Update()
        {
            hideBarTimerCurrent = hideBarTimerCurrent - Time.deltaTime;

            // Do not execute if slider no longer exists
            if (slider == null)
                return;

            HandleBarVisibilty();
        }

        private void LateUpdate()
        {
            HandleCanvasRotation();
        }

        public void SetCurrentBarValue(float _currentValue)
        {
            slider.value = _currentValue;

            // Reset bar timer and make it visible when value is updated
            ResetHideBarTimer();
            HandleBarVisibilty();
        }

        public void SetMaxBarValue(float _maxValue)
        {
            slider.maxValue = _maxValue;
            slider.value = _maxValue;
        }

        public void ResetHideBarTimer()
        {
            hideBarTimerCurrent = hideBarTimerMax;
        }

        private void HandleBarVisibilty()
        {
            if (hideBarTimerCurrent <= 0)
            {
                hideBarTimerCurrent = 0;
                slider.gameObject.SetActive(false);
            }

            else
            {
                if (!slider.gameObject.activeInHierarchy)
                {
                    slider.gameObject.SetActive(true);
                }
            }
        }

        private void HandleCanvasRotation()
        {
            if (slider != null)
            {
                transform.forward = cameraTransform.forward * -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}