using System.Collections;
using PrimeTween;
using UnityEngine;

public class Collectable : MonoBehaviour {
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _pickUpSound;
    [SerializeField] Animator _animator;
    [SerializeField] Collider2D _col;

    void OnValidate() {
        if (!_animator) TryGetComponent(out _animator);
        if (!_col) TryGetComponent(out _col);
    }

    public void Interact() {
        StartCoroutine(InteractProcess());
    }

    IEnumerator InteractProcess() {
        _animator.Play("Base Layer.Interaction");
        _col.enabled = false;
        _source?.PlayOneShot(_pickUpSound);
        Tween.PositionY(transform, transform.position.y + 1f, 0.5f);
        yield return Tween.Alpha(_sprite, 0, 0.5f);
        
        // yield return new WaitForAnimationToFinish(_animator, "Interaction");

        Destroy(gameObject);
    }
}
