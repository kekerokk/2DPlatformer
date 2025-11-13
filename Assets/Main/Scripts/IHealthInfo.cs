using System;
public interface IHealthInfo {
    public int maxHealth { get; }
    public int currentHealth { get; }
    
    public event Action OnHealthDecrease;
    public event Action OnHealthIncrease;
    public event Action OnHealthChange;
}
