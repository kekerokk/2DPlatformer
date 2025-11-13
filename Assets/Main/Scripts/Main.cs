using System;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    [SerializeField] CameraHandler _camera;
    [SerializeField] Player _player;
    [SerializeField] GameUI _gameUI;

    IInput _input;
    Storage _storage;

    [Header("Prefabs")]
    [SerializeField] Player _playerPrefab;
    [SerializeField] CameraHandler _cameraPrefab;
    [SerializeField] GameUI _gameUIPrefab;
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
        _gameUI = Instantiate(_gameUIPrefab);
        _storage = new();
        _input = new InputSystem();
    }
    void Initialize() {
        _storage.Load();

        _camera.Initialize(_player.transform);
        _player.Initialize(_storage, _input);
        _gameUI.Initialize(_input, _player.health, _storage);
        
        _player.OnDied += RestartLevel;
        _input.OnRestart += RestartLevel;
    }
    void StartNextLevel() {
        _storage.Save();
        _currentLevel.levelCompleter.OnComplete -= StartNextLevel;
        Destroy(_currentLevel.gameObject);
        
        // next lvl
        lvlId = ++lvlId >= _levelsPrefabs.Count ? 0 : lvlId;
        BeginLevel(lvlId);
        _camera.ResetPosition();
    }
    void BeginLevel(int id) {
        _currentLevel = Instantiate(_levelsPrefabs[id]);
        _currentLevel.levelCompleter.OnComplete += StartNextLevel;
        _player.transform.position = _currentLevel.spawnPosition.position;
        _camera.SetWorldSize(_currentLevel.GetBounds());
    }
    void RestartLevel() {
        _storage.Load();
        _currentLevel.levelCompleter.OnComplete -= StartNextLevel;
        Destroy(_currentLevel.gameObject);
        
        BeginLevel(lvlId); // or _currentLevel.Restart()
        _player.Restore();
    }
    
    void OnDestroy() {
        if (_input is IDisposable disp) disp.Dispose();
        _player.OnDied -= RestartLevel;
        _input.OnRestart -= RestartLevel;
    }
}
