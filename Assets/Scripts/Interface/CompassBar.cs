using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BON
{
    public class CompassBar : MonoBehaviour
    {
        [Header("Compass")]
        public RectTransform compassBar;

        [Header("3D World Info")]
        public Transform playerTransform;
        public Transform cameraTransform;
        public Transform objectiveTransform;

        [Header("Objective Marker")]
        public RectTransform objectiveMarker;
        public GameObject objectiveMarkerVisual;

        [Header("North Marker")]
        public RectTransform northMarker;
        public GameObject northMarkerVisual;

        [Header("North East Marker")]
        public RectTransform northEastMarker;
        public GameObject northEastMarkerVisual;

        [Header("East Marker")]
        public RectTransform eastMarker;
        public GameObject eastMarkerVisual;

        [Header("South East Marker")]
        public RectTransform southEastMarker;
        public GameObject southEastMarkerVisual;

        [Header("South Marker")]
        public RectTransform southMarker;
        public GameObject southMarkerVisual;

        [Header("South West Marker")]
        public RectTransform southWestMarker;
        public GameObject southWestMarkerVisual;

        [Header("West Marker")]
        public RectTransform westMarker;
        public GameObject westMarkerVisual;

        [Header("North West Marker")]
        public RectTransform northWestMarker;
        public GameObject northWestMarkerVisual;

        // Update is called once per frame
        void Update()
        {
            // Objective Marker
            SetMarkerPosition(objectiveMarker, objectiveTransform.position);

            // Noth Marker
            SetMarkerPosition(northMarker, Vector3.forward * 1000);
            SetMarkerVisibility(northMarker, northMarkerVisual);

            // North East Marker
            SetMarkerPosition(northEastMarker, Vector3.forward * 1000 + Vector3.right * 1000);
            SetMarkerVisibility(northEastMarker, northEastMarkerVisual);

            // East Marker
            SetMarkerPosition(eastMarker, Vector3.right * 1000);
            SetMarkerVisibility(eastMarker, eastMarkerVisual);

            // South East Marker
            SetMarkerPosition(southEastMarker, Vector3.back * 1000 + Vector3.right * 1000);
            SetMarkerVisibility(southEastMarker, southEastMarkerVisual);

            // South Marker
            SetMarkerPosition(southMarker, Vector3.back * 1000);
            SetMarkerVisibility(southMarker, southMarkerVisual);

            // South West Marker
            SetMarkerPosition(southWestMarker, Vector3.back * 1000 + Vector3.left * 1000);
            SetMarkerVisibility(southWestMarker, southWestMarkerVisual);

            // West Marker
            SetMarkerPosition(westMarker, Vector3.left * 1000);
            SetMarkerVisibility(westMarker, westMarkerVisual);

            // North West Marker
            SetMarkerPosition(northWestMarker, Vector3.forward * 1000 + Vector3.left * 1000);
            SetMarkerVisibility(northWestMarker, northWestMarkerVisual);
        }

        private void SetMarkerPosition(RectTransform _marker, Vector3 _worldPosition)
        {
            // Find the direction to the target from the player's transform
            Vector3 directionToTarget = _worldPosition - cameraTransform.position;

            // Find the angle between the target and the camera
            float angle = Vector2.SignedAngle(
                new Vector2(directionToTarget.x, directionToTarget.z), 
                new Vector2(cameraTransform.transform.forward.x, cameraTransform.transform.forward.z));

            // Find the location which the marker should be clamped to on the compass
            float compassPositionX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);

            // Anchor the position of the marker to the compass bar
            _marker.anchoredPosition = new Vector2(compassBar.rect.width / 2 * compassPositionX, 0);
        }

        private void SetMarkerVisibility(RectTransform _marker, GameObject _markerVisual)
        {
            if (_marker.transform.localPosition.x >= 50 ||
                _marker.transform.localPosition.x <= -50)
                _markerVisual.SetActive(false);

            else
                _markerVisual.SetActive(true);
        }
    }
}