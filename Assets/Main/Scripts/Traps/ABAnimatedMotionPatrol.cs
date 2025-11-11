using System.Collections;
using UnityEngine;
public class ABAnimatedMotionPatrol : MonoBehaviour {
    [SerializeField] Transform _object;
    [SerializeField] Animator _animator;
    [SerializeField] Vector2Int _patrolRange;

    [SerializeField] Vector3 _targetPos;
    bool _toRight = true;
    static readonly int _speed = Animator.StringToHash("Speed");

    void Awake() {
        _targetPos = transform.position + new Vector3(_patrolRange.y, 0, 0);
        StartCoroutine(Process());
    }

    IEnumerator Process() {
        while (true) {
            while (Vector3.Distance(_object.position, _targetPos) > 0.02f) {
                yield return null;
            }

            yield return null;

            _toRight = !_toRight;
            _targetPos = transform.position + new Vector3(_toRight ? _patrolRange.y : _patrolRange.x , 0, 0);
            _animator.SetFloat(_speed, _toRight ? 1 : -1);

            yield return null;
        }
    }
}
