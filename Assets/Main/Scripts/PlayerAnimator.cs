using System;
using UnityEngine;
public class PlayerAnimator : MonoBehaviour {
    [SerializeField] Player _player;
    [SerializeField] Animator _animator;
    [SerializeField] PlayerController _controller;
    
    static readonly int _moving = Animator.StringToHash("Moving");
    static readonly int _jumping = Animator.StringToHash("Jumping");
    static readonly int _doubleJumping = Animator.StringToHash("DoubleJumping");
    static readonly int _falling = Animator.StringToHash("Falling");
    const string HIT_ANIM = "Base Layer.Hit";
    const string IDLE = "Base Layer.Idle";
    
    void Update() {
        _animator.SetBool(_moving, _controller.isMoving);
        _animator.SetBool(_jumping, _controller.isJumping);
        _animator.SetBool(_doubleJumping, _controller.isDoubleJumping);
        _animator.SetBool(_falling, _controller.isFalling);
    }

    public void PlayHitAnimation() => _animator.Play(HIT_ANIM);
    public void PlayIdleAnimation() => _animator.Play(IDLE);
}
