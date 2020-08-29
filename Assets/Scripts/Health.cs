using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;

    private int _currentHealth;

    public event Action OnTookHit = delegate { };
    public event Action OnDie = delegate { };
    public event Action<int, int> OnHealthChanged = delegate { };

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void TakeHit(int amount)
    {
        if (_currentHealth <= 0)
            return;

        ModifyHealth(-amount);

        if (_currentHealth > 0)
            OnTookHit?.Invoke();
        else
            OnDie?.Invoke();
    }

    private void ModifyHealth(int amount)
    {
        _currentHealth += amount;
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
    }
}