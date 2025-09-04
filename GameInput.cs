using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions playerInputActions;
    public event EventHandler OnAttack;
    public event EventHandler OnInteract;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        playerInputActions = new PlayerInputActions();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerInputActions.Player.Enable();
        EnableCamera();
        playerInputActions.CameraControl.MouseZoom.performed += CameraController.Instance.Handle_MouseScrolled;
        playerInputActions.Player.Attack.performed += Attack_Performed;
        playerInputActions.Player.Interact.performed += Interact_Performed;
    }

    private void Interact_Performed(InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    public float ReadGamePadZoomValue() => playerInputActions.CameraControl.GamepadZoom.ReadValue<float>();

    private void Attack_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAttack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }

    public void EnableCamera()
    {
        playerInputActions.CameraControl.Enable();
    }

    public void DisableCamera()
    {
        playerInputActions.CameraControl.Disable();
    }

    public void PlayerInputDisable()
    {
        playerInputActions.Player.Disable();
    }

    public void PlayerInputEnable()
    {
        playerInputActions.Player.Enable();
    }
}
