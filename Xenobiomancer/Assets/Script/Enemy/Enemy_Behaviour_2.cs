
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Patterns;

namespace enemySS
{
    public class Enemy_Behaviour_2 : Enemy
    {
        public FSM hfsm;
        public Enemy enemy;
        public GameObject Sp;

        private void Start()
        {
            Sp = Resources.Load<GameObject>("Ability");
            enemy = GameObject.Find("Enemy_1").GetComponent<Enemy>();
            hfsm = new FSM();
            hfsm.Add((int)EnemyStates.PATROL, new Enemy3Patrol(hfsm, (int)(EnemyStates3.PATROL), this));
            hfsm.Add((int)EnemyStates.ATTACKING, new Enemy3Attack(hfsm, (int)(EnemyStates3.ATTACKING), this));
            hfsm.Add((int)EnemyStates.DYING, new Enemy3Dying(hfsm, (int)(EnemyStates3.DYING), this));
            hfsm.SetCurrentState((int)EnemyStates.PATROL);
        }

        private void Update()
        {
            hfsm.Update();
        }

        public void SpAttack()
        {
            // Get the player's position
            Vector2 playerPosition = Player.transform.position;

            // Instantiate the Spike object at the player's position
            GameObject spike = Instantiate(Sp, playerPosition, Quaternion.identity);

            // Get the SpriteRenderer components of the player and the spawned Spike
            SpriteRenderer playerSpriteRenderer = Player.GetComponent<SpriteRenderer>();
            SpriteRenderer spikeSpriteRenderer = spike.GetComponent<SpriteRenderer>();

            // Set the order in layer of the spawned Spike to be higher than the player's
            spikeSpriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder + 1;
        }


    }
    public class EnemyState3 : FSMState
    {
        protected Enemy enemy;
        public EnemyState3(FSM fsm, int id, Enemy enemy) : base(fsm, id)
        {
            this.enemy = enemy;
        }
    }
    public class Enemy3Patrol : EnemyState
    {

        public Enemy3Patrol(FSM fsm, int id, Enemy enemy) : base(fsm, id, enemy)
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

    public class Enemy3Attack : EnemyState
    {
        public Enemy_Behaviour enemyBehaviour;
        public Enemy3Attack(FSM fsm, int id, Enemy enemy) : base(fsm, id, enemy)
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
    public class Enemy3Dying : EnemyState
    {
        public Enemy3Dying(FSM fsm, int id, Enemy enemy) : base(fsm, id, enemy)
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
    public enum EnemyStates3
    {
        PATROL,
        ATTACKING,
        DYING
    }
}
