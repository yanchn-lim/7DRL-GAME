
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Patterns
{
    public class Enemy_Behaviour_2 : Enemy1
    {
        public FSM fsm;
        public GameObject Sp;
        [Header("Sp Settings")]
        [SerializeField]
        private float SpTimer = 0f;
        private float CoolDownInterval = 0.1f;
        [Header("Health Settings")]
        [SerializeField]
        private float currentHealth1;
        [Header("Preotection Settings")]
        [SerializeField]
        private float currentProtectLevel1;
        private bool amorDown = false;
        /// <summary>
        /// made another bool for the armor and the health
        /// </summary>
        [Header("Tracking Settings")]
        private new bool SightLine { get; set; } 

        private void Start()
        {
            Sp = Resources.Load<GameObject>("Ability");
            /*enemy1 = GameObject.Find("Enemy_1").GetComponent<Enemy>();*/
            fsm = new FSM();
           fsm.Add((int)EnemyStates3.PATROL, new Enemy3Patrol(fsm, (int)(EnemyStates3.PATROL), this));
            fsm.Add((int)EnemyStates3.ATTACKING, new Enemy3Attack(fsm, (int)(EnemyStates3.ATTACKING), this));
            fsm.Add((int)EnemyStates3.DYING, new Enemy3Dying(fsm, (int)(EnemyStates3.DYING), this));
            fsm.SetCurrentState((int)EnemyStates3.PATROL);
        }
        /// <summary>
        /// reference to the fms and added the states required.
        /// </summary>

        private void Update()
        {
            /*SightLine = enemy1.SightLine;*/
            fsm.Update();
        }

        public void SpAttack()
        {
            if (this.SightLine)
            {
                SpTimer += Time.deltaTime;
                if (SpTimer >= CoolDownInterval)
                {
                    SpTimer = 0f;
                    // Get the player's position
                    Vector2 playerPosition = Player.transform.position;

                    // Instantiate the Spike object at the player's position
                    Instantiate(Sp, playerPosition, Quaternion.identity);
                    Debug.Log("Spike Generatred");
                    Spike_Enemey spikeScript = Sp.AddComponent<Spike_Enemey>();
                    spikeScript.Functionalized(this);
                    Debug.Log("Spike behaviour generated");
                }
            }
        }
        /// <summary>
        /// Attack function for the enemy behaviour, first it checks if the player is in sight.
        /// in order to timed out the attacked so that multiple instances are not spawned at the same time, there is a cool down time for the each interval 
        /// the enemy gets the player position and then instaniate the prefab.
        /// the prefab behaviour is initalized and added to the prefab.
        /// </summary>
        public override void protectionDown()
        {
            if (isDamageTaken)
            {
                var enemy_currentProtection2 = enemy_Stats.max_Protection - 10f;
                Debug.Log($"Current Health Level: {enemy_currentProtection2}");
                if (enemy_currentProtection2 < enemy_Stats.min_Protection)
                {
                    DamageonHealth();
                }
                else
                {
                    currentProtectLevel1 = enemy_currentProtection2;
                }
            }
        }
        /// <summary>
        /// refer to enemy1. protection down
        /// i had an override method is because i wanted each script behave seperately and not with one script leading the other two scipts
        /// as normally without the override,the method is not updated to the newest enemy and the codition above will occur.
        /// </summary>

        public override void DamageonHealth()
        {
            var enemy_currentHealth = enemy_Stats.max_Health - 10;
            Debug.Log($"Current Health Level: {enemy_currentHealth}");
            if(enemy_currentHealth < enemy_Stats.min_Health )
            {
                fsm.SetCurrentState((int)EnemyStates.DYING);
            }
            else
            {
                enemy_currentHealth = currentHealth1;
            }
        }
        /// <summary>
        /// refer to enemy1.cs.protection down
        /// </summary>

        public override void StillInSight()
        {
            base.StillInSight();
        }

        public override void FindPlayer()
        {
            base.FindPlayer();
        }


    }
    public class EnemyState3 : FSMState
    {
        public Enemy_Behaviour_2 enemy2;
        
        public EnemyState3(FSM fsm, int id, Enemy_Behaviour_2 enemy) : base(fsm, id)
        {
            this.enemy2 = enemy;
        }
    }
    /// <summary>
    /// the fsm which i added changes to make the fsm used this behaviour script instead of something else.
    /// </summary>
    public class Enemy3Patrol : EnemyState3
    {

        public Enemy3Patrol(FSM fsm, int id, Enemy_Behaviour_2 enemy) : base(fsm, id, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            enemy2.FindPlayer();
            Debug.Log("Finding Player");
            if (enemy2.distanceToPlayer <= enemy2.enemy_Stats.RangeofSight)
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

    public class Enemy3Attack : EnemyState3
    {
        
        public Enemy3Attack(FSM fsm, int id, Enemy_Behaviour_2 enemy) : base(fsm, id, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            enemy2.StillInSight();
            enemy2.SpAttack();
            Debug.Log("Sp Attack");
            enemy2.protectionDown();
            enemy2.DamageonHealth();
            
           
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
    public class Enemy3Dying : EnemyState3
    {
        public Enemy3Dying(FSM fsm, int id, Enemy_Behaviour_2 enemy) : base(fsm, id, enemy)
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
