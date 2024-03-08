using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Patterns
{
    public class Enemy : MonoBehaviour
    {
        public Enemy_Stats enemy_Stats;
        public GameObject Player;
        public bool isDamageTaken = false;
        public bool SightLine = false;
        public float currentHealth;
        public float currentProtectLevel;
        public bool amrorDown = false;
        public float distanceToPlayer;
        public Player_Attack playerAttack;
        public GameObject Bullet;

        //public StateMachine stateMachine;
        public FSM fsm;

        private void Start()
        {
            playerAttack = Player.GetComponent<Player_Attack>();
            Bullet = playerAttack.bulletPrefab;
            
            Player = GameObject.Find("Player");
            fsm = new FSM();
            fsm.Add((int)EnemyStates.PATROL, new EnemyPatrolling(fsm, (int)(EnemyStates.PATROL), this));
            fsm.Add((int)EnemyStates.ATTACKING, new EnemyAttack(fsm, (int)(EnemyStates.ATTACKING), this));
            fsm.Add((int)EnemyStates.DYING, new EnemyDying(fsm, (int)(EnemyStates.DYING), this));
            fsm.SetCurrentState((int)EnemyStates.PATROL);
        }

        private void Update()
        {
            fsm.Update();
        }

        public void DamageonHealth()
        {
            if (isDamageTaken)
            {
                currentHealth = enemy_Stats.current_Health - 10f;
                Debug.Log($"Current Health Level: {currentHealth}");

                if (currentHealth <= enemy_Stats.min_Health)
                {
                    
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
                isDamageTaken = true;
                // Call this immediately upon getting hit
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Bullet"))
            {
                Destroy(other.gameObject);
                Debug.Log("Trigger exited");
                isDamageTaken = false;
            }
        }

        public void FindPlayer()
        {
            distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
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
                    //move towards the end of the red raycast


                }
            }
        }


        public void protectionDown()
        {
            if (isDamageTaken)
            {
                currentProtectLevel = enemy_Stats.protectionLevel - 10f;
                Debug.Log($"Current Protection Level: {currentProtectLevel}");
                if (currentProtectLevel <= 0)
                {
                    amrorDown = true;
                    DamageonHealth();
                    amrorDown = false;

                }
                else
                {
                    enemy_Stats.protectionLevel = currentProtectLevel;
                }
            }
        }

        public void ResetStats()
        {
            enemy_Stats.current_Health = enemy_Stats.max_Health;
            enemy_Stats.protectionLevel = enemy_Stats.max_Protection;
        }



    }

    /*   public class StateMachine
       {
           public IState CurrentState { get; private set; }
           public PatrolState patrolState;
           public AttackState attackState;
           public TakeDamageState takeDamageState;
           public DeathState deathState;

           public void Initialize(IState startingState)
           {
               CurrentState = startingState;
               startingState.Enter();
           }
           public void TransitionTo(IState nextState)
           {
               CurrentState.Exit();
               CurrentState = nextState;
               nextState.Enter();
           }
           public void Update()
           {
               if (CurrentState != null)
               {
                   CurrentState.Update();
               }
           }
       }


   }



   public interface IState
   {
       public void Enter()
       {
           // code that runs when we first enter the state
       }
       public void Update()
       {
           // per-frame logic, include condition to transition to a new
           //state
       }
       public void Exit()
       {
           // code that runs when we exit the state
       }
   }*/

    public class EnemyState : FSMState
    {
        protected Enemy enemy;
        public EnemyState(FSM fsm, int id, Enemy enemy) : base(fsm, id)
        {
            this.enemy = enemy;
        }
    }
    public class EnemyPatrolling : EnemyState
    {
        public EnemyPatrolling(FSM fsm, int id, Enemy enemy) : base(fsm, id, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            enemy.FindPlayer();
            if (enemy.distanceToPlayer <= enemy.enemy_Stats.RangeofSight)
            {
                mFsm.SetCurrentState((int)EnemyStates.ATTACKING);
                Debug.Log("Starting Chase");
            }
            enemy.protectionDown();

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

    public class EnemyAttack : EnemyState
    {
        public EnemyAttack(FSM fsm, int id, Enemy enemy) : base(fsm, id, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            enemy.StartledAndMove();
            enemy.StillInSight();
            enemy.protectionDown();
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
    public class EnemyDying : EnemyState
    {
        public EnemyDying(FSM fsm, int id, Enemy enemy) : base(fsm, id, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            GameObject.Destroy(enemy.gameObject);
            enemy.ResetStats();
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
    public enum EnemyStates
    {
        PATROL,
        ATTACKING,
        DYING
    }
}


