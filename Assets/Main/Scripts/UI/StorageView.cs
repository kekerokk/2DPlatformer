using System;
using TMPro;
using UnityEngine;

public class StorageView : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _countText;

    IStorageInfo _storage;
    
    public void Initialize(IStorageInfo storage) {
        _storage = storage;
        _storage.MoneyChanged += Refresh;
        Refresh();
    }
    void Refresh() {
        _countText.SetText(_storage.money.ToString());
    }

    void OnDestroy() => _storage.MoneyChanged -= Refresh;
}
