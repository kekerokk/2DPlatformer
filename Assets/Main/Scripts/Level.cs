using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour {
    [SerializeField] Grid _grid;
    [SerializeField] Tilemap _tilemap;
    [field: SerializeField] public LevelCompleter levelCompleter { get; private set; }
    [field: SerializeField] public Transform spawnPosition { get; private set; }
    
    void Awake() {
        Debug.Log($"MapSize: {_tilemap.size}");
    }
}
