using Patterns;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace enemyT
{
    public class NormalEnemy : EnemyBase
    {
        [Header("Damage")]
        [Tooltip("how many hp the enemy dealt to the player")]
        [SerializeField] protected int damage;
        [Tooltip("how many times the enemy can attack in one turn")]
        [SerializeField] protected int damagePerRound;
        [Tooltip("how close the enemy must be to damage the player")]
        [SerializeField] protected float damageRadius;

        #region getter
        public int Damage { get => damage; }
        public float DamageRadius { get => damageRadius; }
        public int DamagePerRound { get => damagePerRound; }
        #endregion

        protected override void DamagePlayer()
        {
            throw new System.NotImplementedException();
        }

        protected override void Update()
        {
            base.Update();
            //if(path != null)
            //{
            //    print("path count: "path.count);
            //}
        }

        protected override void SetupFSM()
        {
            fsm = new FSM();
            fsm.Add((int)EnemyState.IDLE, new NormalEnemyIdleState(fsm, this));
            fsm.Add((int)EnemyState.CHASING, new BasicChasingEnemyState(fsm, this));
            fsm.SetCurrentState((int)EnemyState.IDLE);
            
        }

        protected override void StartDeath()
        {
            //do show the death animation here
            throw new System.NotImplementedException();
        }

        private void OnDrawGizmos()
        {
            if(path == null)
            {
                print("path empty");
                return;
            }
            if (path.Count == 0 )
            {
                print("no path");
                return;
            }
            print($"current there are {path.Count}");
            Stack<Vector2> debugDummy = new Stack<Vector2>(path);
            Vector2 previous = debugDummy.Pop();
            while (debugDummy.Count > 0)
            {
                Vector2 position = debugDummy.Pop();
                Debug.DrawLine(previous, position, UnityEngine.Color.yellow, 1f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(previous, 0.1f);
                previous = position;
            }

            Gizmos.DrawSphere(transform.position, PointSensingRadius);
        }
    }

}