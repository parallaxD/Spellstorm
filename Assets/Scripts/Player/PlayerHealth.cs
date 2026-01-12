using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;

    public static event Action<int> OnHealthChanged;       
    public event Action<int> OnHealthReduced;        
    public event Action<int> OnHealthRestored;       
    public event Action OnDeath;                    
    public event Action<int, int> OnHealthChangedWithMax;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public float HealthPercentage => (float)_currentHealth / _maxHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(_currentHealth);
        OnHealthChangedWithMax?.Invoke(_currentHealth, _maxHealth);
    }

    public void ReduceHealth(int value)
    {
        if (value <= 0 || !IsAlive()) return;

        int previousHealth = _currentHealth;
        _currentHealth = Mathf.Max(0, _currentHealth - value);

        OnHealthChanged?.Invoke(_currentHealth);
        OnHealthReduced?.Invoke(value);
        OnHealthChangedWithMax?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void RestoreHealth(int value)
    {
        if (value <= 0 || !IsAlive()) return;

        int previousHealth = _currentHealth;
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + value);

        OnHealthChanged?.Invoke(_currentHealth);
        OnHealthRestored?.Invoke(value);
        OnHealthChangedWithMax?.Invoke(_currentHealth, _maxHealth);
    }

    public void SetMaxHealth(int newMaxHealth, bool restoreToMax = false)
    {
        if (newMaxHealth <= 0) return;

        _maxHealth = newMaxHealth;

        if (restoreToMax)
        {
            _currentHealth = _maxHealth;
        }
        else
        {
            _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
        }

        OnHealthChanged?.Invoke(_currentHealth);
        OnHealthChangedWithMax?.Invoke(_currentHealth, _maxHealth);
    }

    public bool IsAlive()
    {
        return _currentHealth > 0;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

}
