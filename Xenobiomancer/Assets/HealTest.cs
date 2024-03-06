using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTest : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.IncreaseHealth(10);
        }
    }
}
