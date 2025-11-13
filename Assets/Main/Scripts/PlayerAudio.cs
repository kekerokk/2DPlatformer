using System;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    [SerializeField] AudioSource _audio;
    [SerializeField] PlayerController _controller;
    Health _health;

    [Header("Clips")]
    [SerializeField] AudioClip _hurt;
    [SerializeField] AudioClip _jump;

    public void Initialize(Health health) {
        _health = health;
        _health.OnHealthDecrease += PlayHurt;
        _controller.OnJump += PlayJump;
    }
    void OnDestroy() {
        _health.OnHealthDecrease -= PlayHurt;
        _controller.OnJump -= PlayJump;
    }

    void PlayHurt() => _audio.PlayOneShot(_hurt);
    void PlayJump() => _audio.PlayOneShot(_jump);
}
