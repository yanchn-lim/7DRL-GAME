using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public abstract class Stats: MonoBehaviour
{
    private float health;
    private float maxHealth;
    private float currency;
    private float travelDistance;

    #region Getters and Setters
    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float Currency
    {
        get { return currency; }
        set { currency = value; }
    }
    #endregion

    public virtual void InitializeStats(float initialHealth, float maxHealth, float initialCurrency)
    {
        Health = initialHealth;
        MaxHealth = maxHealth;
        Currency = initialCurrency;
    }

    public virtual void IncreaseHealth(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0f, MaxHealth);
    }

    public virtual void DecreaseHealth(float amount)
    {
        Health -= amount;
        Health = Mathf.Clamp(Health, 0f, MaxHealth);
    }

    public virtual void IncreaseCurrency(float amount)
    {
        Currency += amount;
    }

    public virtual void DecreaseCurrency(float amount)
    {
        Currency -= amount;
        Currency = Mathf.Max(0f, Currency);
    }

}
