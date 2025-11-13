using UnityEngine;

public class CameraHandler : MonoBehaviour {
    [SerializeField] Camera _cam;
    [SerializeField] Transform _player;

    [Header("Params")]
    [SerializeField] float _nearPlayerDistance;
    [SerializeField] float _moveSmooth = 0.5f;

    BoundsInt _levelBounds;
    Vector3 _camMinBounds, _camMaxBounds;

    Vector2 _velocity;

    void OnValidate() {
        if (!_cam) TryGetComponent(out _cam);
    }

    public void Initialize(Transform player) {
        if (!_cam) _cam = Camera.main;
        _player = player;
        
        _camMinBounds = _cam.ViewportToWorldPoint(new Vector3(0, 0, _cam.transform.position.z));
        _camMaxBounds = _cam.ViewportToWorldPoint(new Vector3(1, 1, _cam.transform.position.z));
    }

    void Update() {
        Vector3 camWorldCenter = _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
        
        if(!(Vector3.Distance(camWorldCenter, _player.position) > _nearPlayerDistance)) return;

        Vector3 targetPos = Vector2.SmoothDamp(camWorldCenter, _player.position, ref _velocity, _moveSmooth);

        targetPos.y = Mathf.Clamp(targetPos.y, _levelBounds.yMin + _camMaxBounds.y, _levelBounds.yMax - _camMaxBounds.y);
        targetPos.x = Mathf.Clamp(targetPos.x, _levelBounds.xMin + _camMaxBounds.x, _levelBounds.xMax - _camMaxBounds.x);
        
        _cam.transform.position = targetPos.WithZ(_cam.transform.position.z);
    }

    public void SetWorldSize(BoundsInt size) => _levelBounds = size;
    public void ResetPosition() {
        Vector3 position = _player.position;
        position.y = Mathf.Clamp(position.y, _levelBounds.yMin + _camMaxBounds.y, _levelBounds.yMax - _camMaxBounds.y);
        position.x = Mathf.Clamp(position.x, _levelBounds.xMin + _camMaxBounds.x, _levelBounds.xMax - _camMaxBounds.x);
        
        _cam.transform.position = position.WithZ(_cam.transform.position.z);
    }
}
