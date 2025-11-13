using System;
using UnityEngine;

[Serializable]
public class Storage : IStorageInfo {
    // Storage and Saves in one for now
    [field: SerializeField] public int money { get; private set; }

    public event Action MoneyChanged = delegate { };

    const string MONEY = "money";
    
    public void Load() {
        money = PlayerPrefs.GetInt(MONEY, 0);
        MoneyChanged.Invoke();
    }
    public void Save() => PlayerPrefs.SetInt(MONEY, money);

    public void Add(int count = 1) {
        money += 1;
        MoneyChanged.Invoke();
    }
}
