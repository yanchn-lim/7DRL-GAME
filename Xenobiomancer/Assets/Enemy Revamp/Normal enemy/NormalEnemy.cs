using Patterns;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;

namespace enemyT
{
    public class NormalEnemy : EnemyBase
    {

        protected override void DamagePlayer()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetupFSM()
        {
            fsm = new FSM();
            fsm.Add((int)EnemyState.IDLE, new BasicIdleEnemyState(fsm, this));
            fsm.Add((int)EnemyState.CHASING, new BasicChasingEnemyState(fsm, this));
            fsm.SetCurrentState((int)EnemyState.IDLE);
            
        }

        protected override void StartDeath()
        {
            //do show the death animation here
            throw new System.NotImplementedException();
        }

    }


    public enum EnemyState
    {
        IDLE,
        CHASING,
        
        ATTACKSTATE
    }

}