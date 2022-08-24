using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Direction")]
        public float horizontal;
        public float vertical;

        [Header("Mouse")]
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        private PlayerControls inputActions;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        public void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

    }
}
