using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour {
    [SerializeField] Grid _grid;
    [SerializeField] Tilemap _tilemap;
    [field: SerializeField] public LevelCompleter levelCompleter { get; private set; }
    [field: SerializeField] public Transform spawnPosition { get; private set; }
    
    void Awake() {
        _tilemap.CompressBounds();
        Debug.Log($"TilemapSize: {_tilemap.size}, TileCenter: {_tilemap.cellBounds.center}");
    }

    public BoundsInt GetBounds() {
        _tilemap.CompressBounds();
        return _tilemap.cellBounds;
    }
}
