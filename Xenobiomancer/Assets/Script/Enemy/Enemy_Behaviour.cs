using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns
{
    public class Enemy_Behaviour : Enemy
    {
        public FSM zfsm;
        public Enemy enemy;
        public GameObject Bullet_enemy;
        private float retaliationTimer = 0f;
        public float retaliationInterval = 0.1f;

        private void Start()
        {
            Bullet_enemy = Resources.Load<GameObject>("enemy_Bullet");
            enemy = GameObject.Find("Enemy_1").GetComponent<Enemy>();
            zfsm = new FSM();
            zfsm.Add((int)EnemyStates.PATROL, new Enemy2Patrol(zfsm, (int)(EnemyStates2.PATROL), this));
            zfsm.Add((int)EnemyStates.ATTACKING, new Enemy2Attack(zfsm, (int)(EnemyStates2.ATTACKING), this));
            zfsm.Add((int)EnemyStates.DYING, new Enemy2Dying(zfsm, (int)(EnemyStates2.DYING), this));
            zfsm.SetCurrentState((int)EnemyStates.PATROL);
        }

        private void Update()
        {
            zfsm.Update();
        }

        public void Retaliation()
        {
            /*            Vector3 directionToPlayer = Player.transform.position - transform.position;


                        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

                        // Instantiate a bullet in the direction the player is facing
                        GameObject bullet = Instantiate(Bullet_enemy, transform.position, Quaternion.Euler(0, 0, angle));

                        // Set the velocity of the bullet
                        bullet.GetComponent<Rigidbody2D>().velocity = directionToPlayer.normalized * 4f;*/

            /*            Vector3 directionToPlayer = Player.transform.position - transform.position;
                        directionToPlayer.Normalize(); // Normalize to get a unit vector

                        // Calculate the angle between the forward direction of the enemy and the direction to the player
                        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

                        // Instantiate the bullet prefab at the enemy's position
                        GameObject bullet = Instantiate(Bullet_enemy, transform.position, Quaternion.Euler(0, 0, angle));

                        // Set the bullet's velocity based on the direction
                        bullet.GetComponent<Rigidbody2D>().velocity = directionToPlayer * 2f;*/

            if (enemy.SightLine)
            {
                retaliationTimer += Time.deltaTime;
                if (retaliationTimer >= retaliationInterval)
                {
                    // Reset the timer
                    retaliationTimer = 0f;

                    Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;


                    // Instantiate the bullet prefab at the enemy's position
                    GameObject bullet = Instantiate(Bullet_enemy, transform.position, Quaternion.identity);

                    // Get the Rigidbody2D component from the bullet
                    Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

                    // Set the velocity of the bullet to move in the calculated direction
                    bulletRb.velocity = directionToPlayer * 3f;

                    Bullet_Enemy bulletScript = bullet.AddComponent<Bullet_Enemy>();
                    bulletScript.Initialize(this);

                }

            }
        }

    }
    public class EnemyState2 : FSMState
    {
        protected Enemy enemy;
        public EnemyState2(FSM fsm, int id, Enemy enemy) : base(fsm, id)
        {
            this.enemy = enemy;
        }
    }
    public class Enemy2Patrol : EnemyState
    { 

        public Enemy2Patrol(FSM fsm, int id, Enemy enemy) : base(fsm, id, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            enemy.FindPlayer();
            Debug.Log("Finding Player");
            if (enemy.distanceToPlayer <= enemy.enemy_Stats.RangeofSight)
            {
                mFsm.SetCurrentState((int)EnemyStates.ATTACKING);
                Debug.Log("Starting Chase");
            }
            
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }

    public class Enemy2Attack : EnemyState
    {
        public Enemy_Behaviour enemyBehaviour;
        public Enemy2Attack(FSM fsm, int id, Enemy enemy) : base(fsm, id, enemy)
        {
            enemyBehaviour = enemy as Enemy_Behaviour;
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            enemy.StillInSight();
            enemyBehaviour.Retaliation();
            if (enemy.amrorDown)
            {
                enemy.currentHealth = enemy.enemy_Stats.current_Health - 1;
                Debug.Log($"Current Health Level: {enemy.currentHealth}");

                if (enemy.currentHealth <= enemy.enemy_Stats.min_Health)
                {
                    mFsm.SetCurrentState((int)EnemyStates.DYING);
                }
                else
                {
                    enemy.enemy_Stats.current_Health = enemy.currentHealth;
                }

            }
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
    public class Enemy2Dying : EnemyState
    {
        public Enemy2Dying(FSM fsm, int id, Enemy enemy) : base(fsm, id, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
    public enum EnemyStates2
    {
        PATROL,
        ATTACKING,
        DYING
    }
}




