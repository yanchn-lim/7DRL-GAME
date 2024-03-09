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

        [Header("Pathfinding and Chasing")]
        [Tooltip("how close the enemy must be from the point of the path inorder to be assign a new one")]
        [SerializeField] protected float pointSensingRadius;
        [Tooltip("The distance in which allow the enemy to fully know where the player is for chasing")]
        [SerializeField] protected float playerSensingRadius;

       

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
        //public int DamagePerRound { get => damagePerRound; }
        public float DegreeOfVision { get => degreeOfVision; }
        public float LengthOfVision { get => lengthOfVision; }
        public Player Player { get => player; }
        public float RotationSpeed { get => rotationSpeed;  }
        public Stack<Vector2> Path { get => path; set => path = value; }
        public float PointSensingRadius { get => pointSensingRadius; }
        public float PlayerSensingRadius { get => playerSensingRadius; }
        #endregion

        protected virtual void Start() //for starting the enemy
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy"); //the layermask the player is in
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); //get the player component for finding
            SetupFSM();
        }

        protected virtual void Update()
        {
            //GridHelper.Instance.GeneratePath(transform.position, Player.transform.position);
            fsm.Update();
        }
        protected abstract void SetupFSM();


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


