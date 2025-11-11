using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    [SerializeField] CameraHandler _camera;
    [SerializeField] Player _player;

    [Header("Prefabs")]
    [SerializeField] Player _playerPrefab;
    [SerializeField] CameraHandler _cameraPrefab;
    [SerializeField] List<Level> _levelsPrefabs;
    [SerializeField] int lvlId = 0;

    Level _currentLevel;
    
    void Awake() {
        Create();
        Initialize();
        BeginLevel(lvlId);
    }
    void Create() {
        _player = Instantiate(_playerPrefab);
        _camera = Instantiate(_cameraPrefab);
    }
    void Initialize() {
        _player.OnDied += RestartLevel;
    }
    void StartNextLevel() {
        _currentLevel.levelCompleter.OnComplete -= StartNextLevel;
        Destroy(_currentLevel.gameObject);
        
        // next lvl
        lvlId = ++lvlId >= _levelsPrefabs.Count ? 0 : lvlId;
        BeginLevel(lvlId);
    }
    void BeginLevel(int id) {
        _currentLevel = Instantiate(_levelsPrefabs[id]);
        _currentLevel.levelCompleter.OnComplete += StartNextLevel;
        _player.transform.position = _currentLevel.spawnPosition.position;
    }
    void RestartLevel() {
        _currentLevel.levelCompleter.OnComplete -= StartNextLevel;
        Destroy(_currentLevel.gameObject);
        BeginLevel(lvlId);
        _player.Restore();
    }
}
