using System;
using UnityEngine;

[Serializable]
public class PlayerController : MonoBehaviour {
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] CapsuleCollider2D _col; 
    [SerializeField] Transform _body;
    
    [Header("Movement")]
    [SerializeField] float _maxSpeed = 6f;
    [SerializeField] float _jumpHeight = 7;
    [SerializeField] float _toGroundDistance = 0.05f;
    [SerializeField] float _acceleration = 60;
    [SerializeField] float _groundDeceleration = 60f, _airDeceleration = 40f;
    [SerializeField] float _maxFallSpeed = 15;
    [SerializeField] float _inAirGravity = 15f;
    [SerializeField] LayerMask _groundMask;

    [Header("Runtime")]
    [SerializeField] bool _active = true;
    [SerializeField] bool _isOnGround = true;
    [SerializeField] bool _airJumped;
    [SerializeField] bool _jumpPerformed = false;
    [SerializeField] bool _sideHitted, _ceilHitted;
    [SerializeField] Vector2 _frameVelocity;
    [SerializeField] Vector2 _move;
    bool _resetRequires;
    
    public bool isMoving { get; private set; }
    public bool isJumping { get; private set; }
    public bool isFalling { get; private set; }
    public bool isDoubleJumping { get; private set; }
    public bool isGrounded => _isOnGround;

    public event Action OnJump = delegate { };

    public void UpdateInput(ref MoveInputData input) {
        if(!_active) return;
        
        if (input.jumpForced) _jumpPerformed = true;
        _move = input.move;
    }
    public void Updt() {
        if (_resetRequires) {
            _frameVelocity = Vector2.zero;
            _resetRequires = false;
        }
        
        GroundCheck();
        
        if(!_active) return;
        
        Movement();
        ForceJump();
        Rotation();
        Gravity();
        
        ApplyMovement();
    }
    void GroundCheck() {
        _sideHitted = false;

        var groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size * _col.transform.lossyScale, _col.direction, 0,
            Vector2.down, _toGroundDistance, _groundMask);
        RaycastHit2D _ceilHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size * _col.transform.lossyScale, _col.direction, 0,
            Vector2.up, _toGroundDistance, _groundMask);  
        var sideHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size * _col.transform.lossyScale, _col.direction, 0,
            _body.right, _toGroundDistance, _groundMask);

        if (_ceilHit != default && Vector2.Angle(_body.up,(new Vector3(_ceilHit.point.x, _ceilHit.point.y)- _body.position)) < 45) 
            _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
        if (sideHit != default && Vector2.Angle(_body.right,(new Vector3(sideHit.point.x, sideHit.point.y)- _body.position)) < 30) {
            _frameVelocity.x = 0;
            _sideHitted = true;
        }

        _isOnGround = groundHit && groundHit.point.y <= _body.position.y;

        if (_isOnGround) {
            _airJumped = false;
            isJumping = false;
            isDoubleJumping = false;
            isFalling = false;
        } else {
            isFalling = _rb.linearVelocityY < -0.0001f;
        }
    }
    void Movement() {
        isMoving = !_sideHitted && _move.x != 0;
        if (_sideHitted) {
            return;
        }
        
        if (_move.x == 0) {
            var deceleration = _isOnGround ? _groundDeceleration : _airDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _move.x * _maxSpeed, _acceleration * Time.fixedDeltaTime);
        }
    }
    void ForceJump() {
        if (_jumpPerformed) {
            if (_isOnGround || _airJumped == false) {
                _airJumped = !_isOnGround;
                _frameVelocity.y = _jumpHeight;
                isDoubleJumping = _airJumped;
                isJumping = true;
                OnJump.Invoke();
            }
        }
        
        _jumpPerformed = false;
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
    public void Reset() {
        _active = true;
        _resetRequires = true;
    }
}
