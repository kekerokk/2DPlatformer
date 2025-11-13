using UnityEngine;
public class GameUI : MonoBehaviour {
    [SerializeField] RestartView _restartView;
    [SerializeField] PlayerHealthView _healthView;
    [SerializeField] StorageView _storageView;
    
    public void Initialize(IInput input, IHealthInfo health, IStorageInfo storage) {
        _restartView.Initialize(input);
        _healthView.Initialize(health);
        _storageView.Initialize(storage);
    }
}
