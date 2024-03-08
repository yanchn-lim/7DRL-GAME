using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
///Same pattern throughout the scripts, refer to enemy_behaviour_2.cs
namespace Patterns
{
    public class Enemy : Enemy1
    {

        public bool isDamage = false;
        public bool SightLine1 = false;
        public float currentHealth2;
        public float currentProtectLevel2;
        public bool amrorDown1 = false;
        public float distanceToPlayer1;
        public Player_Attack playerAttack1;
        public GameObject Bullet1;

        //public StateMachine stateMachine;
        public FSM fsm;

        private void Start()
        {
            playerAttack1 = Player.GetComponent<Player_Attack>();
            Bullet1 = playerAttack1.bulletPrefab;
            currentHealth = 100;
            currentProtectLevel = 100;
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

        public override void protectionDown()
        {
            if (isDamageTaken)
            {
                var enemy_currentProtection = enemy_Stats.max_Protection - 10f;
                Debug.Log($"Current Health Level: {enemy_currentProtection}");
                if (enemy_currentProtection < enemy_Stats.min_Protection)
                {
                    amrorDown1 = true;
                    DamageonHealth();
                    amrorDown1 = false;
                }
                else
                {
                    currentProtectLevel2 = enemy_currentProtection;
                }
            }
        }

        public override void DamageonHealth()
        {
            if (amrorDown)
            {
                var enemy3_healthCurrent = enemy_Stats.max_Health - 10;
                Debug.Log($"Current Health Level: {enemy3_healthCurrent}");

                if (enemy3_healthCurrent <= enemy_Stats.min_Health)
                {
                    fsm.SetCurrentState((int)EnemyStates.DYING);
                }
                else
                {
                    enemy3_healthCurrent = currentHealth2;
                }

            }
        }

        public override void StillInSight()
        {
            base.StillInSight();
        }

        public override void FindPlayer()
        {
            base.FindPlayer();
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
        public Enemy enemy; 
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
            enemy.DamageonHealth(); 
            

            
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


