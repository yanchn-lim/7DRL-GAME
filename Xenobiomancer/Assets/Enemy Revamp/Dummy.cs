using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour , IDamageable
{
    public void TakeDamage(int damage)
    {
        print($"{name} take damage");
    }

    
}
