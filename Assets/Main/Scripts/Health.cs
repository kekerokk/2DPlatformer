using System;
using UnityEngine;

[Serializable]
public class Health: IHealthInfo {
    [field: SerializeField] public int maxHealth { get; private set; } = 1;
    [field: SerializeField] public int current { get; private set; }
    public int currentHealth => current;

    public bool isMaxHeath => maxHealth == current;

    public event Action OnZeroHealth = delegate { };
    public event Action OnHealthDecrease = delegate { };
    public event Action OnHealthIncrease = delegate { };
    public event Action OnHealthChange = delegate { };

    public void Reset() {
        current = maxHealth;
        OnHealthChange.Invoke();
    }

    public void Decrease(int count = 1) {
        current = Mathf.Clamp(current - count, 0, maxHealth);
        OnHealthDecrease.Invoke();

        if (current == 0) 
            OnZeroHealth.Invoke();
    }
    public void Increase(int count = 1) {
        current = Mathf.Clamp(current + count, 0, maxHealth);
        
        OnHealthIncrease.Invoke();
    }
}
