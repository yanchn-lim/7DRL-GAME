using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public abstract class Enemy1 : MonoBehaviour
{
    //creating the abstract class and method so that calling them would be easier
    public Enemy_Stats enemy_Stats;//referencing to abstract class
    public GameObject Player;// referencing to player to get position
    public bool isDamageTaken = false;//checking if player is damaging enemy
    public new bool SightLine = false;// checking if the sightline exist to change raycasting behaviour
    public float currentHealth;// store the enemy's current health
    public float currentProtectLevel;// store the enemy's current protection level
    public bool amrorDown = false;//checking if the amror is depeleted
    public float distanceToPlayer;//checking the distance from enemy to player
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Trigger entered");
            isDamageTaken = true;
            // Call this immediately upon getting hit
        }
    }
    //check collision between enemy and player bullet, if the gameobject has a tag call bullet, the player is damaging the enemy.

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Debug.Log("Trigger exited");
            isDamageTaken = false;
        }
    }
    //checks whether bullet is still in collision with enemy, if so the bullet will be destroyed and the player will not be damaging enemy

    public virtual void FindPlayer()
    {
        distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
    }

    // set the distance from enemy to player.
    public virtual void StartledAndMove()
    {
        if (SightLine)
        {
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            transform.Translate(direction * enemy_Stats.MovementSpeed * Time.deltaTime);
        }

    }
    // moves to player when player still in sightline(sight line bool)
    public void MoveToPlayer()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        transform.Translate(direction * enemy_Stats.MovementSpeed * Time.deltaTime);
    }

    public virtual void StillInSight()
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
                //move towards the end of the red raycast


            }
        }
    }
    /// <summary>
    /// casts a raycast from the enemy position to the distance between the player and the enemy, if the raycast hits the player's collider
    /// the info is store in the sightline bool and will draw a ray in green
    /// otherwise if any toher collider it will draw in red.
    /// </summary>


    public virtual void protectionDown()
    {
        if (isDamageTaken)
        {
            var protectionCurrent = enemy_Stats.max_Protection - 10f;
            Debug.Log($"Current Protection Level: {protectionCurrent}");
            if (protectionCurrent <= enemy_Stats.min_Protection)
            {
                amrorDown = true;
                DamageonHealth();
                amrorDown = false;

            }
            else
            {
                protectionCurrent = currentProtectLevel;
            }
        }
    }
    /// <summary>
    /// When the damage is taken, there would be temporary bool that stores the current protection level of the remaining pretection.
    /// And will log it out, if the current level is lesser than the min_protection required, the bool armor down will be actived to 
    /// show that the shield is no more and the damage on health function will start running.
    /// the bool armor down is changed back to false to prevent  from running multiple times
    /// the bool armor down is changed back to false to prevent DamageonHealth from running multiple times.
    /// </summary>

    public void ResetStats()
    {
         currentHealth = enemy_Stats.max_Health;
        currentProtectLevel = enemy_Stats.max_Protection;
    }
    /// <summary>
    /// resetting the health and protectionLevel after enemy died.
    /// </summary>
    public virtual void DamageonHealth()
    {
        if (amrorDown)
        {
            var healthCurrent = enemy_Stats.max_Health - 10f;
            Debug.Log($"Current Health Level: {healthCurrent}");



            if (healthCurrent <= enemy_Stats.min_Health)
            {
                //Dying
            }
            else
            {
                healthCurrent = currentHealth;
            }
        }
    }
    /// The damage on health is called when the when armor is down, the current health which is taken from the max_health is minused 10f
    /// after which is logged out and if the temporary stored health is less than the min health recorded
    /// the dying method is called or else the currentHealth is updated.
}
