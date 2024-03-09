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
            if (PlayerWithinVision() || enemyReference.Path != null || PlayerWithinSenseRange())
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