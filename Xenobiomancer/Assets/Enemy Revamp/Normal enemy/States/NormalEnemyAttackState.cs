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
        public NormalEnemyAttackState(FSM fsm, NormalEnemy enemy) : base(fsm, enemy)
        {
            this.enemyReference = enemy; 
            mId = (int)EnemyState.ATTACKSTATE;
        }

        public override void Enter()
        {
            base.Enter();
            waitTimeForNextAttack = normalEnemyReference.DamagePerRound / GameManager.Instance.TurnTime;
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
            playerReference.TakeDamage(normalEnemyReference.Damage);
        }

    }

}