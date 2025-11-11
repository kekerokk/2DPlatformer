using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public readonly ref struct MoveInputData {
    public readonly Vector2 move;
    public readonly bool jumpForced;
    public MoveInputData(Vector2 move, bool jumpForced) {
        this.move = move;
        this.jumpForced = jumpForced;
    }
}

public class Player : MonoBehaviour {
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] CapsuleCollider2D _col;
    [SerializeField] Transform _playerBody;
    [SerializeField] Animator _animator;
    [SerializeField] Health _health;
    [SerializeField] Sword _sword;
    [SerializeReference, SubclassSelector] IController _controller;

    public Health health => _health;
    
    [SerializeField] float _hitForce = 4f;
    [SerializeField] float _hitDuration = 0.5f;
    [SerializeField] LayerMask _excludeDangerous;
    

    [Header("Input")] // todo interface for input, class for InputSystem
    [SerializeField] Vector2 _moveDirection;
    InputActions _input;
    InputActions.PlayerActions _playerActions;
    
    static readonly int _moving = Animator.StringToHash("Moving");
    static readonly int _jumping = Animator.StringToHash("Jumping");
    static readonly int _doubleJumping = Animator.StringToHash("DoubleJumping");
    static readonly int _falling = Animator.StringToHash("Falling");
    static readonly int _hitted = Animator.StringToHash("Hitted");
    static readonly string _hitAnim = "Base Layer.Hit";

    void Awake() {
        _input = new();
        _playerActions = _input.Player;
        _playerActions.Attack.performed += Attack; 
        _playerActions.Enable();
        _health.Reset();
    }
    void Attack(InputAction.CallbackContext obj) {
        _sword.Attack();
    }
    void Update() {
        MoveInputData input = new(_playerActions.Movement.ReadValue<Vector2>(), _playerActions.Jump.WasPerformedThisFrame());
        _controller.UpdateInput(ref input);
    }
    void FixedUpdate() {
        _controller.Update();
        UpdateAnimator();
    }
    void UpdateAnimator() {
        _animator.SetBool(_moving, _controller.isMoving);
        _animator.SetBool(_jumping, _controller.isJumping);
        _animator.SetBool(_doubleJumping, _controller.isDoubleJumping);
        _animator.SetBool(_falling, _controller.isFalling);
    }
    IEnumerator HitProcess(Vector2 direction) {
        _controller.Disable();
        _animator.SetBool(_hitted, true);
        _animator.Play(_hitAnim);
        _rb.AddForce((direction + Vector2.up) * _hitForce, ForceMode2D.Impulse);
        _rb.excludeLayers = _excludeDangerous;

        yield return new WaitForSeconds(_hitDuration);

        _rb.excludeLayers = default;
        _animator.SetBool(_hitted, false);
        _controller.Enable();
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Collect some items
        if (other.gameObject.CompareTag("Heal") && !_health.isMaxHeath) {
            _health.Increase();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Money")) {
            // Receive 
        }
    }
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Trap")) {
            health.Decrease();
            StartCoroutine(HitProcess((_playerBody.position.WithY(0) - other.transform.position.WithY(0)).normalized));
        }
    }
    void OnDestroy() {
        if(_input == null) return;  
        _playerActions.Disable();
    }
}
