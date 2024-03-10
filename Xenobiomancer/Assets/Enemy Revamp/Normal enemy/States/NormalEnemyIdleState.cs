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
            Debug.Log($"{enemyReference.name} is deciding state");
            Debug.Log($"Playerwithin range: {playerWithinVision}. Playerwithin senserange {playerWithinSenseRange}");
            if (playerWithinVision || 
                enemyReference.Path != null || 
                playerWithinSenseRange ||
                enemyReference.TookDamage
                )
            {
                Debug.Log($"{enemyReference.name} is decided to chase");
                mFsm.SetCurrentState((int)EnemyState.CHASING);
            }
            else
            {
                Debug.Log($"{enemyReference.name} is decided to idle");
                mFsm.SetCurrentState((int)(EnemyState.IDLE));
            }
        }
    }
}