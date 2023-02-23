using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class EnemyHealthBar : MonoBehaviour
    {
        public Transform cameraTransform;
        private Slider slider;

        private float hideBarTimerCurrent = 0;
        private float hideBarTimerMax = 10;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        private void Update()
        {
            hideBarTimerCurrent = hideBarTimerCurrent - Time.deltaTime;

            Debug.Log("Enemy Health Bar Timer:" + hideBarTimerCurrent);
            HandleHealthBarVisibilty();
            HandleHealthBarOnDeath();
        }

        private void LateUpdate()
        {
            HandleCanvasRotation();
        }

        public void SetCurrentHealthBarValue(float _health)
        {
            slider.value = _health;

            // Reset bar timer and make it visible when health bar is updated
            ResetHideBarTimer();
            HandleHealthBarVisibilty();
        }

        public void SetMaxHealthBarValue(float _maxHealth)
        {
            slider.maxValue = _maxHealth;
            slider.value = _maxHealth;
        }

        public void ResetHideBarTimer()
        {
            hideBarTimerCurrent = hideBarTimerMax;
        }

        private void HandleHealthBarVisibilty()
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

        private void HandleHealthBarOnDeath()
        {
            if (slider.value <= 0)
            {
                Destroy(slider.gameObject);
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