using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _playerBody;
    [SerializeField] Animator _animator;
    [SerializeField] Health _health;
    [SerializeField] Sword _sword; 
    public Health health => _health;
    
    [Header("Movement")]
    [SerializeField] float _speed = 4f;
    [SerializeField] float _jumpHeight = 3;
    [SerializeField] float _toGroundDistance = 1f;
    [SerializeField] bool _isOnGround = true;
    [SerializeField] bool _airJumped;
    [SerializeField] float _hitDuration = 0.5f;
    int _groundMask;

    [Header("Input")] // todo interface for input, class for InputSystem
    [SerializeField] Vector2 _moveDirection;
    InputActions _input;
    InputActions.PlayerActions _playerActions;
    bool _inputEnabled = true;
    
    RaycastHit2D[] _hit = new RaycastHit2D[1];
    
    static readonly int _moving = Animator.StringToHash("Moving");
    static readonly int _jumping = Animator.StringToHash("Jumping");
    static readonly int _doubleJumping = Animator.StringToHash("DoubleJumping");
    static readonly int _falling = Animator.StringToHash("Falling");
    static readonly int _hitted = Animator.StringToHash("Hitted");

    void Awake() {
        _groundMask = LayerMask.GetMask(new[] { "Ground" });
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
        GroundCheck();
        
        if(_inputEnabled == false) return;
        ForceJump();
        Movement();
        Rotation();
    }
    
    void Movement() {
        _moveDirection = _playerActions.Movement.ReadValue<Vector2>();
        _rb.linearVelocityX = _moveDirection.x * _speed;
        _animator.SetBool(_moving, _moveDirection.x != 0);
    }
    IEnumerator JumpBuffer() {
        yield break;
    }
    void Rotation() {
        if (_moveDirection.x != 0) {
            _playerBody.rotation = Quaternion.Euler(0, _moveDirection.x < 0 ? 180 : 0, 0);
        }
    }

    // todo => IEnumerator GroundChecker();
    void GroundCheck() {
        _isOnGround = Physics2D.RaycastNonAlloc(transform.position, -transform.up, _hit, _toGroundDistance,
            _groundMask) > 0;

        if (_isOnGround) {
            _airJumped = false;
            _animator.SetBool(_jumping, false);
            _animator.SetBool(_doubleJumping, false);
        } 
        
        _animator.SetBool(_falling, _rb.linearVelocityY < -0.0001f);
    }
    void ForceJump() {
        if (_playerActions.Jump.WasPerformedThisFrame()) {
            if (_isOnGround || _airJumped == false) {
                _rb.AddForceY((_isOnGround ? _jumpHeight : _jumpHeight / 2f) * 100f);
                _airJumped = !_isOnGround;
                _animator.SetBool(_doubleJumping, _airJumped);
                _animator.SetBool(_jumping, true);
            }
        }
    }
    IEnumerator HitProcess(Vector2 direction) {
        _inputEnabled = false;
        _animator.SetBool(_hitted, true);
        _animator.Play("Base Layer.Hit");
        _rb.linearVelocityX = 0;
        _rb.linearVelocityY = 0;
        _rb.AddForce((direction + Vector2.up) * 180f);

        yield return new WaitForSeconds(_hitDuration);
        
        while (_isOnGround == false) {
            yield return null;
        }

        _animator.SetBool(_hitted, false);
        _inputEnabled = true;
    }


    void OnTriggerEnter(Collider other) {
        // Collect some items
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
