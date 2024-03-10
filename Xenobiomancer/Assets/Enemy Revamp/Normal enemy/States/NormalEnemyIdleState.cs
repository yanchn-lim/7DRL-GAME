using Patterns;
using System.Collections;
using UnityEngine;

namespace enemyT
{
    public class NormalEnemyIdleState : BasicIdleEnemyState
    {
        public NormalEnemyIdleState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
        }

        


        protected override void DecideNextState()
        {

            if (playerWithinVision || 
                enemyReference.Path != null || 
                playerWithinSenseRange ||
                enemyReference.TookDamage
                )
            {
                mFsm.SetCurrentState((int)EnemyState.CHASING);
            }
            else
            {
                mFsm.SetCurrentState((int)(EnemyState.IDLE));
            }
        }
    }
}