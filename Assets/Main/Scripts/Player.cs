using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _playerBody;
    [SerializeField] PlayerAnimator _animator;
    [SerializeField] PlayerController _controller;
    [SerializeField] PlayerAudio _audio;
    [SerializeField] Health _health;
    [SerializeField] Sword _sword;
    Storage _storage;
    public Health health => _health;
    
    [Header("Hit Params")]
    [SerializeField] float _hitForce = 4f;
    [SerializeField] float _hitDuration = 0.5f;
    [SerializeField] LayerMask _excludeDangerous;

    IInput _input;

    public event Action OnDied = delegate { };

    public void Initialize(Storage storage, IInput input) {
        _storage = storage;
        _input = input;
        _audio.Initialize(_health);
        
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
    void FixedUpdate() => _controller.Updt();
    IEnumerator HitProcess(Vector2 direction) {
        _controller.Disable();
        _animator.PlayHitAnimation();
        
        _rb.AddForce((direction + Vector2.up) * _hitForce, ForceMode2D.Impulse);
        _rb.excludeLayers = _excludeDangerous;

        yield return new WaitForSeconds(_hitDuration);
        yield return new WaitWhile(() => _controller.isGrounded == false);

        _rb.excludeLayers = default;
        _controller.Enable();
    }
    void Die() => OnDied.Invoke();

    public void Restore() {
        _health.Reset();
        _controller.Reset();
        _animator.PlayIdleAnimation();
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