// BulletScript.cs
using Patterns;
using System.Collections;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    private Enemy_Behaviour enemy;

    public void Initialize(Enemy_Behaviour enemy)
    {
        this.enemy = enemy;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with a wall
        if (other.gameObject.CompareTag("Wall")||other.gameObject.CompareTag("Enemy"))
        {
            // Destroy the bullet
            Destroy(gameObject);
        }
    }

}

public class Spike_Enemey : MonoBehaviour
{
    private Enemy_Behaviour_2 enemy;

    public void Functionalized(Enemy_Behaviour_2 enemy)
    {
        this.enemy = enemy;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DestroySpike(1f));
        }
    }
    IEnumerator DestroySpike(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Destroy(gameObject);
    }
}

