using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthView : MonoBehaviour {
    [SerializeField] Image _hpPrefab;
    [SerializeField] RectTransform _container;
    [SerializeField] List<Image> _healthImgs;

    IHealthInfo _health;

    public void Initialize(IHealthInfo health) {
        _health = health;
        _healthImgs = new(health.maxHealth);
        for (int i = 0; i < health.maxHealth; i++) {
            _healthImgs.Add(Instantiate(_hpPrefab, _container));
        }
        
        health.OnHealthIncrease += AddHealth;
        health.OnHealthDecrease += RemoveHealth;
        health.OnHealthChange += ChangeHealth;
    }
    void OnDestroy() {
        _health.OnHealthIncrease -= AddHealth;
        _health.OnHealthDecrease -= RemoveHealth;
        _health.OnHealthChange -= ChangeHealth;
    }

    void AddHealth() {
        // Heal Effect
        
        ChangeHealth();
    }
    void RemoveHealth() {
        // Hurt effect
        
        ChangeHealth();
    }
    void ChangeHealth() {
        for (int i = 0; i < _health.maxHealth; i++) {
            _healthImgs[i].gameObject.SetActive(i < _health.currentHealth);
        }
    }
}
