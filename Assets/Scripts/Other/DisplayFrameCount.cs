using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BON
{
    public class DisplayFrameCount : MonoBehaviour
    {
        [Header("General")]
        public TextMeshProUGUI frameCountText;
        public float pollingTime = 1f;

        private float _time;
        private int _frameCount;

        // Update is called once per frame
        void Update()
        {
            _time += Time.deltaTime;

            _frameCount++;

            if (_time > -pollingTime)
            {
                int frameRate = Mathf.RoundToInt(_frameCount / _time);
                frameCountText.text = "FPS: " + frameRate.ToString();
            }
        }
    }
}