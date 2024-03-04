using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public Enemy_Stats enemy_Stats;
    public Behaviours currentBehavior;
    public GameObject Player;
    public bool isDamageTaken = false;
    public bool SightLine = false;
    public float currentHealth;
    public float currentProtectLevel;


    public void DamageonHealth()
    {
        if (isDamageTaken)
        {
            currentHealth = enemy_Stats.current_Health - 1;
            Debug.Log($"Current Health Level: {currentHealth}");

            if (currentHealth <= enemy_Stats.min_Health)
            {
                currentBehavior = Behaviours.Death;
            }
            else
            {
                enemy_Stats.current_Health = currentHealth;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Trigger entered");
            currentBehavior = Behaviours.TakingDamage;
            DamageonHealth();
            isDamageTaken = true;
            currentBehavior = Behaviours.Attacking;// Call this immediately upon getting hit
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Trigger exited");
            isDamageTaken = false;
        }
    }

    public void FindPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer <= enemy_Stats.RangeofSight)
        {
            currentBehavior = Behaviours.Attacking;
        }
    }


    public void StartledAndMove()
    {
        if (SightLine)
        {
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            transform.Translate(direction * enemy_Stats.MovementSpeed * Time.deltaTime);
        }

    }

    public void MoveToPlayer()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        transform.Translate(direction * enemy_Stats.MovementSpeed * Time.deltaTime);
    }

    public void StillInSight()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Player.transform.position - transform.position);
        if (ray.collider != null)
        {
            SightLine = ray.collider.CompareTag("Player");
            if (SightLine)
            {
                Debug.DrawRay(transform.position, Player.transform.position - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, Player.transform.position - transform.position, Color.red);
            }
        }
    }


    public void protectionDown()
    {
        currentProtectLevel = enemy_Stats.protectionLevel - 10;
        Debug.Log($"Current Protection Level: {currentProtectLevel}");
        if (currentProtectLevel <= 0)
        {
            DamageonHealth();
        }
        else
        {
            enemy_Stats.protectionLevel = currentProtectLevel;
        }
    }

    public void ResetStats()
    {
        enemy_Stats.current_Health = enemy_Stats.max_Health;
        enemy_Stats.protectionLevel = enemy_Stats.max_Protection;
    }
}
