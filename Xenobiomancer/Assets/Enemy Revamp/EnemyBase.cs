using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace enemyT
{
    //set the data on the prefab directly dont use scriptable object
    public abstract class EnemyBase : MonoBehaviour , IDamageable
    {
        [Tooltip("the hp of the enemy")]
        [SerializeField] protected int health;
        [SerializeField] protected float speed;
        [SerializeField] protected float rotationSpeed;
        [Tooltip("how many times the enemy can attack in one turn")]
        [SerializeField] protected int damage;
        [Tooltip("how many times the enemy can attack in one turn")]
        [SerializeField] protected int damagePerRound;

        [Header("vision")]
        [Tooltip("Vision range")]
        [SerializeField] protected float degreeOfVision;
        [Tooltip("length of the vision")]
        [SerializeField] protected float lengthOfVision;
        protected Stack<Vector2> path;

        private Player player;
        protected FSM fsm;

        #region getter
        public int Health { get => health; }
        public float Speed { get => speed; }
        public int Damage { get => damage; }
        public int DamagePerRound { get => damagePerRound; }
        public float DegreeOfVision { get => degreeOfVision; }
        public float LengthOfVision { get => lengthOfVision; }
        public Player Player { get => player; }
        public float RotationSpeed { get => rotationSpeed;  }
        public Stack<Vector2> Path { get => path; set => path = value; }
        #endregion

        protected virtual void Start() //for starting the enemy
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); //get the player component for finding
            SetupFSM();
        }

        protected virtual void Update()
        {
            GridHelper.Instance.GeneratePath(transform.position, Player.transform.position);
            fsm.Update();
        }
        protected abstract void SetupFSM();

        protected abstract void DamagePlayer();

        protected abstract void StartDeath();

        public virtual void TakeDamage(int damage)
        {
            health -= damage;
            if(health < 0)
            {
                StartDeath();
            }
        }
    }

    public enum EnemyStates
    {
        PATROL,
        FINDING,
        ATTACKING,
        DYING
    }
}


