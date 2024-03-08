// BulletScript.cs
using Patterns;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    private Enemy_Behaviour enemy;

    public void Initialize(Enemy_Behaviour enemy)
    {
        this.enemy = enemy;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet collided with a wall
        if (collision.gameObject.CompareTag("Wall")||collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the bullet
            Destroy(gameObject);
        }
    }

}

