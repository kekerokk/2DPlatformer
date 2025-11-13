using System;
using UnityEngine;

public class ABPatrol : MonoBehaviour {
    [SerializeField] InterfaceReference<IPatroller> _patroller;
    [SerializeField] Transform APoint, BPoint;

    bool _isAPoint;
    Vector3 _targetPos;

    void Awake() {
        _patroller.Value.OnDestroyEvt += Destroy;
        _targetPos = BPoint.position;
        _patroller.Value.SetTarget(_targetPos);
        _isAPoint = false;
    }
    void Destroy() {
        Destroy(gameObject);
    }

    void Update() {
        if (Vector3.Distance(_patroller.Value.position.WithY(0), _targetPos.WithY(0)) < 0.1f) {
            _targetPos = _isAPoint ? BPoint.position : APoint.position;
            _isAPoint = !_isAPoint;
            _patroller.Value.SetTarget(_targetPos);
        }
    }
}