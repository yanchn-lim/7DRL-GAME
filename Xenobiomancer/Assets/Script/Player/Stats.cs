using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats: MonoBehaviour
{
    public float Health;
    public float MaxHealth;

    public float Currency;

    public abstract void IncreaseValue();
    public abstract void DecreaseValue();
    
}
