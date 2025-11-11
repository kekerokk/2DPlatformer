using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IPatroller {
    [SerializeField] Rigidbody2D _rb;
    [SerializeReference, SubclassSelector] IController _controller;
    [SerializeField] Health _health;
    [SerializeField] float _hitForce = 4f;
    [SerializeField] float _hitDuration = 0.5f;

    [Header("Runtime")]
    [SerializeField] Vector3 _target;

    public Vector3 position => transform.position;
    public event Action OnDestroyEvt = delegate { };

    void Awake() {
        _health.Reset();
        _health.OnZeroHealth += Death;
    }
    void OnDestroy() {
        _health.OnZeroHealth -= Death;
    }
    void Death() {
        OnDestroyEvt.Invoke();
        Destroy(gameObject);
    }

    public void SetTarget(Vector3 pos) {
        _target = pos;
    }

    void Update() {
        MoveInputData input = new((_target - transform.position).normalized, false);
        _controller.UpdateInput(ref input);
    }
    void FixedUpdate() {
        _controller.Update();
    }
    
    IEnumerator HitProcess(Vector2 direction) {
        _controller.Disable();
        _rb.AddForce((direction + Vector2.up) * _hitForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_hitDuration);
        yield return new WaitWhile(() => _controller.isGrounded == false);

        _controller.Enable();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Weapon")) {
            _health.Decrease();
            StartCoroutine(HitProcess((transform.position.WithY(0) - other.transform.position.WithY(0)).normalized));
        }
    }
}
