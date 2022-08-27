using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BON
{
    public class DisplayFrameCount : MonoBehaviour
    {
        public TextMeshProUGUI FpsText;

        public float pollingTime = 1f;
        private float time;
        private int frameCount;

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;

            frameCount++;

            if (time > -pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                FpsText.text = "FPS: " + frameRate.ToString();
            }
        }
    }
}