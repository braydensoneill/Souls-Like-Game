using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class Crosshair : MonoBehaviour
    {
        private CameraHandler cameraManager;
        public Image icon;

        private void Awake()
        {
            cameraManager = FindObjectOfType<CameraHandler>();
        }

        private void Update()
        {
            SetCrosshairVisibility();
        }

        public void SetCrosshairVisibility()
        {
            if (cameraManager.currentLockOnTarget == null)
                icon.gameObject.SetActive(true);

            else
                icon.gameObject.SetActive(false);
        }
    }
}

