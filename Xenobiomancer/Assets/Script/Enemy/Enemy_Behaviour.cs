using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///same pattern throughout the scripts, refer to enemy_behaviour_2
namespace Patterns
{
    public class Enemy_Behaviour : Enemy
    {
        public FSM zfsm;
        public GameObject Bullet_enemy;
        private float retaliationTimer = 0f;
        public float retaliationInterval = 0.1f;
        public float currentHealth3;
        public float currentProtectLevel3;
        private bool amorDown;

        public new bool SightLine { get; private set; }

        private void Start()
        {
            Bullet_enemy = Resources.Load<GameObject>("enemy_Bullet");
            fsm = new FSM();
            fsm.Add((int)EnemyStates2.PATROL1, new Patrol2(fsm, (int)(EnemyStates2.PATROL1), this));
            fsm.Add((int)EnemyStates2.ATTACKING1, new Attack1(fsm, (int)(EnemyStates2.ATTACKING1), this));
            fsm.Add((int)EnemyStates2.DYING1, new Dying1(fsm, (int)(EnemyStates2.DYING1), this));
            fsm.SetCurrentState((int)EnemyStates.PATROL);
            currentHealth2 = 100;
            currentProtectLevel2 = 100;
        }

        private void Update()
        {
            fsm.Update();
        }

        public void Retaliation()
        {

            if (this.SightLine)
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

        public override void protectionDown()
        {
            if (isDamageTaken)
            {
                var enemy_currentProtection1 = enemy_Stats.max_Protection - 10f;
                Debug.Log($"Current Health Level: {enemy_currentProtection1}");
                if (enemy_currentProtection1 < enemy_Stats.min_Protection)
                {
                    amorDown = true;
                    DamageonHealth();
                    amorDown = false;
                }
                else
                {
                    currentProtectLevel3 = enemy_currentProtection1;
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
                    enemy3_healthCurrent = currentHealth3;
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
    public class EnemyState2 : FSMState
    {
        protected Enemy_Behaviour enemy1;
        public EnemyState2(FSM fsm, int id, Enemy_Behaviour enemy) : base(fsm, id)
        {
            this.enemy1 = enemy;
        }
    }
    public class Patrol2 : EnemyState2
    { 

        public Patrol2(FSM fsm, int id, Enemy_Behaviour enemy) : base(fsm, id, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            enemy1.FindPlayer();
            Debug.Log("Finding Player");
            if (enemy1.distanceToPlayer <= enemy1.enemy_Stats.RangeofSight)
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

    public class Attack1 : EnemyState2
    {
        public Attack1(FSM fsm, int id, Enemy_Behaviour enemy) : base(fsm, id, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            enemy1.StillInSight();
            enemy1.Retaliation();
            enemy1.protectionDown();
            enemy1.DamageonHealth();

            
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
    public class Dying1 : EnemyState2
    {
        public Dying1(FSM fsm, int id, Enemy_Behaviour enemy) : base(fsm, id, enemy)
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
        PATROL1,
        ATTACKING1,
        DYING1
    }
}




