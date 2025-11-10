using System.Collections;
using PrimeTween;
using UnityEngine;

public class Sword : MonoBehaviour {
    [SerializeField] Transform _center;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Collider2D _collider;

    [Header("Params")]
    [SerializeField] float _attackDuration;

    bool _attacking;
    
    public void Attack() {
        if(_attacking) return;
        _attacking = true;
        StartCoroutine(AttackProcess());
    }

    IEnumerator AttackProcess() {
        _collider.enabled = true;
        _sprite.color = _sprite.color.WithA(1);

        yield return Tween.LocalRotation(_center, Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, -90, 0), _attackDuration);
        
        _collider.enabled = false;
        _sprite.color = _sprite.color.WithA(0);
        _attacking = false;
    }
}
