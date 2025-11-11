using UnityEngine;
public class CameraHandler : MonoBehaviour {
    [SerializeField] Camera _cam;

    void OnValidate() {
        if (!_cam) TryGetComponent(out _cam);
    }

    void Awake() {
        if (!_cam) _cam = Camera.main;
        
        Vector3 lowestX = _cam.ScreenToWorldPoint(new Vector3(0, 0, 0));

        Debug.Log($"lowestX: {lowestX}");
    }
}
