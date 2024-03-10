using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Stats: MonoBehaviour
{
    private int health;
    private int maxHealth;
    private int currency;
    protected float travelDistance;

    #region Getters and Setters
  
    public float TravelDistance
    {
        get { return travelDistance; }
        set { travelDistance = value;  }

    }
    protected int Health { get => health; set => health = value; }
    protected int MaxHealth { get => maxHealth; set => maxHealth = value; }
    protected int Currency { get => currency; set => currency = value; }
    #endregion

    public virtual void InitializeStats(int initialHealth, int maxHealth, int initialCurrency, float intialTravelDistance)
    {
        Health = initialHealth;
        MaxHealth = maxHealth;
        Currency = initialCurrency;
        TravelDistance = intialTravelDistance;
    }

    public virtual void IncreaseHealth(int amount)
    {
        Health += amount;
        Health = math.clamp(Health, 0,maxHealth );
    }

    public virtual void DecreaseHealth(int amount)
    {
        Health -= amount;
        Health = math.clamp(Health, 0, MaxHealth);
        if(health == 0)
        {
            SceneManager.LoadScene(3);
        }
    }

    public virtual void IncreaseCurrency(int amount)
    {
        Currency += amount;
    }

    public virtual void DecreaseCurrency(int amount)
    {
        Currency -= amount;
        Currency = math.max(0, Currency);
    }

    public bool CanSpendAmount(int amount)
    {
        if( currency >= amount )
        {
            DecreaseCurrency(amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void IncreaseTravelDistance(float amount)
    {
        TravelDistance += amount;
    }

    public virtual void DecreaseTravelDistance(float amount)
    {
        TravelDistance -= amount;
    }

}
