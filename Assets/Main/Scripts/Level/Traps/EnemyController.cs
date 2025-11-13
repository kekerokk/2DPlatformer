using UnityEngine;
public class EnemyController : MonoBehaviour {
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] CapsuleCollider2D _col; 
    [SerializeField] Transform _body;
    
    [Header("Movement")]
    [SerializeField] float _maxSpeed = 6f;
    [SerializeField] float _toGroundDistance = 0.05f;
    [SerializeField] float _acceleration = 60;
    [SerializeField] float _groundDeceleration = 60f, _airDeceleration = 40f;
    [SerializeField] float _maxFallSpeed = 15;
    [SerializeField] float _inAirGravity = 15f;
    [SerializeField] LayerMask _groundMask;

    [Header("Runtime")]
    [SerializeField] bool _active = true;
    [SerializeField] bool _isOnGround = true;
    [SerializeField] Vector2 _frameVelocity;
    [SerializeField] Vector2 _move;
    
    public bool isGrounded => _isOnGround;

    public void UpdateInput(ref MoveInputData input) {
        if(!_active) return;
        
        _move = input.move;
    }
    public void Update() {
        GroundCheck();
        
        if(!_active) return;
        
        Movement();
        Rotation();
        Gravity();
        
        ApplyMovement();
    }
    void GroundCheck() {
        var groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size * _col.transform.lossyScale, _col.direction, 0,
            Vector2.down, _toGroundDistance, _groundMask);

        _isOnGround = groundHit && groundHit.point.y <= _body.position.y;
    }
    void Movement() {
        if (_move.x == 0) {
            var deceleration = _isOnGround ? _groundDeceleration : _airDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _move.x * _maxSpeed, _acceleration * Time.fixedDeltaTime);
        }
    }
    void Rotation() {
        if (_move.x != 0) {
            _body.rotation = Quaternion.Euler(0, _move.x < 0 ? 180 : 0, 0);
        }
    }
    void Gravity() {
        if (_isOnGround && _frameVelocity.y <= 0f) {
            _frameVelocity.y = -1;
        }
        else {
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_maxFallSpeed, _inAirGravity * Time.fixedDeltaTime);
        }
    }
    void ApplyMovement() => _rb.linearVelocity = _frameVelocity;

    public void Enable() => _active = true;
    public void Disable() {
        _rb.linearVelocityX = 0;
        _rb.linearVelocityY = 0;
        _active = false;
    }
}
