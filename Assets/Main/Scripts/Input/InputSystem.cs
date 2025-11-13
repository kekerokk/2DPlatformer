using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputSystem : IInput, IDisposable {
    public Vector2 move => _playerActions.Movement.ReadValue<Vector2>();
    public bool isJump => _playerActions.Jump.WasPerformedThisFrame();

    public event Action OnAttack = delegate { };
    public event Action OnPause = delegate { };
    public event Action OnRestart = delegate { };
    public event Action OnRestartCancelled = delegate { };
    public event Action<float> OnRestartStarted = delegate { };
    
    InputActions _input;
    InputActions.PlayerActions _playerActions;
    
    public InputSystem() {
        _input = new();
        _playerActions = _input.Player;
        _playerActions.Attack.performed += InvokeAttack;
        _playerActions.Pause.performed += Pause;
        _playerActions.Restart.started += RestartStart;
        _playerActions.Restart.canceled += RestartCancelled;
        _playerActions.Restart.performed += RestartPerformed;
        _playerActions.Enable();
    }
    void Pause(InputAction.CallbackContext obj) => OnPause.Invoke();
    void RestartStart(InputAction.CallbackContext obj) => OnRestartStarted.Invoke(Time.time);
    void RestartCancelled(InputAction.CallbackContext obj) => OnRestartCancelled.Invoke();
    void RestartPerformed(InputAction.CallbackContext obj) {
        Debug.Log($"Restart Performed");
        OnRestart.Invoke();
    }

    void InvokeAttack(InputAction.CallbackContext obj) => OnAttack.Invoke();

    public void Dispose() {
        _input.Dispose();
    }
}
