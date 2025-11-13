using System.Collections;
using Dev.ComradeVanti.WaitForAnim;
using UnityEngine;

public class Collectable : MonoBehaviour {
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
        
        yield return new WaitForAnimationToFinish(_animator, "Interaction");

        Destroy(gameObject);
    }
}
