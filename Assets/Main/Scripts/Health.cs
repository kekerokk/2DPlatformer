using System;
using UnityEngine;
[Serializable]
public class Health {
    [field: SerializeField] public int maxHealth { get; private set; } = 1;
    [field: SerializeField] public int current { get; private set; }

    public event Action OnZeroHealth = delegate { };
    public event Action OnHealthDecrease = delegate { };

    public void Reset() {
        current = maxHealth;
    }

    public void Decrease(int count = 1) {
        current = Mathf.Clamp(current - count, 0, maxHealth);
        OnHealthDecrease.Invoke();

        if (current == 0) 
            OnZeroHealth.Invoke();
    }
}
