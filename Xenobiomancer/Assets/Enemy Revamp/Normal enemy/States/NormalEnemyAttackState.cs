using Bioweapon;
using Patterns;
using System.Collections;
using UnityEngine;

namespace enemyT
{
    public class NormalEnemyAttackState : BasicEnemyState
    {
        private NormalEnemy normalEnemyReference;
        private float elapseTime;
        private float waitTimeForNextAttack;
        private Animator animator { get {  return NormalEnemy.FindAnyObjectByType<Animator>(); } }
        public NormalEnemyAttackState(FSM fsm, NormalEnemy enemy) : base(fsm, enemy)
        {
            normalEnemyReference = enemy; 
            mId = (int)EnemyState.ATTACKSTATE;
        }

        public override void Enter()
        {
            base.Enter();
            waitTimeForNextAttack = GameManager.Instance.TurnTime / normalEnemyReference.DamagePerRound ;
            elapseTime = 0f;
            AttackPlayer();
        }

        public override void Update()
        {
            if (PlayerWithinVision())
            {
                //if player within sight
                if(elapseTime > waitTimeForNextAttack)
                {
                    AttackPlayer();
                    animator.SetFloat("PosX", 2);
                    animator.SetFloat("PosY", 2);
                    elapseTime = 0f;
                }
                else
                {
                    elapseTime += Time.deltaTime;
                }
            }
            else
            {
                //return back to chasing the player to get near to the player
                mFsm.SetCurrentState((int)EnemyState.CHASING);
            }
        }

        private void AttackPlayer()
        {
            enemyReference.Player.TakeDamage(normalEnemyReference.Damage);
        }
    }

}