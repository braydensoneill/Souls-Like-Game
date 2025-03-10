using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    private PlayerControls inputActions;

    private void OnEnable()
    {
        inputActions = new PlayerControls();
        inputActions.PlayerActions.LockOn.performed += OnLockOnPerformed;
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnLockOnPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("LockOn input received in test script");
    }
}
