using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 5;

    private int currentHealth;

    public event Action OnTookHit = delegate { };

    public event Action OnDie = delegate { };

    public event Action<int, int> OnHealthChanged = delegate { };

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeHit(int amount)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        ModifyHealth(-amount);

        if (currentHealth > 0)
        {
            OnTookHit();
        }
        else
        {
            OnDie();
        }
    }

    private void ModifyHealth(int amount)
    {
        currentHealth += amount;
        OnHealthChanged(currentHealth, maxHealth);
    }
}