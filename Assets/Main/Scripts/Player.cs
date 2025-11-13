using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] CapsuleCollider2D _col;
    [SerializeField] Transform _playerBody;
    [SerializeField] Animator _animator;
    [SerializeField] Health _health;
    [SerializeField] Sword _sword;
    [SerializeReference, SubclassSelector] IController _controller;
    Storage _storage;
    public Health health => _health;
    
    [SerializeField] float _hitForce = 4f;
    [SerializeField] float _hitDuration = 0.5f;
    [SerializeField] LayerMask _excludeDangerous;

    IInput _input;

    public event Action OnDied = delegate { };
    
    static readonly int _moving = Animator.StringToHash("Moving");
    static readonly int _jumping = Animator.StringToHash("Jumping");
    static readonly int _doubleJumping = Animator.StringToHash("DoubleJumping");
    static readonly int _falling = Animator.StringToHash("Falling");
    static readonly string _hitAnim = "Base Layer.Hit";

    public void Initialize(Storage storage, IInput input) {
        _storage = storage;
        _input = input;
        
        _input.OnAttack += Attack;
        _health.Reset();
        _health.OnZeroHealth += Die;
    }
    void OnDestroy() {
        _input.OnAttack -= Attack;
        _health.OnZeroHealth -= Die;
    }
    void Attack() {
        _sword.Attack();
    }
    void Update() {
        MoveInputData input = new(_input.move, _input.isJump);
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
        _animator.Play(_hitAnim);
        _rb.AddForce((direction + Vector2.up) * _hitForce, ForceMode2D.Impulse);
        _rb.excludeLayers = _excludeDangerous;

        yield return new WaitForSeconds(_hitDuration);
        yield return new WaitWhile(() => _controller.isGrounded == false);

        _rb.excludeLayers = default;
        _controller.Enable();
    }
    void Die() {
        OnDied.Invoke();
    }

    public void Restore() {
        _health.Reset();
        _controller.Reset();
        _animator.Play("Base Layer.Idle");
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Collect some items
        if (other.gameObject.CompareTag("Heal") && !_health.isMaxHeath) {
            _health.Increase();
            if(other.TryGetComponent(out Collectable collectable)) 
                collectable.Interact();
        }
        if (other.gameObject.CompareTag("Money")) {
            _storage.Add();
            if (other.TryGetComponent(out Collectable collectable))
                collectable.Interact();
        }
        if (other.gameObject.CompareTag("FallDeath")) {
            Die();
        }
    }
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Trap")) {
            if (_health.current - 1 > 0) StartCoroutine(HitProcess((_playerBody.position.WithY(0) - other.transform.position.WithY(0)).normalized));
            _health.Decrease();
        }
    }
}