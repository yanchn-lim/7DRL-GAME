using Patterns;
using System.Collections;
using UnityEngine;

namespace enemyT
{
    public class BossIdleState : BasicIdleEnemyState
    {
        private BossEnemy boss;

        public BossIdleState(FSM fsm, BossEnemy enemy) : base(fsm, enemy)
        {
            mId = (int)EnemyState.IDLE;
            boss = enemy;
        }

        protected override void DecideNextState()
        {
            float distance = Vector2.Distance(transform.position, playerReference.transform.position);
            if(distance > boss.AttackableRange)
            {//if the player is out of distance then chase after them
                mFsm.SetCurrentState((int)EnemyState.CHASING);
            }
            else
            {//else can start doing the attack
                mFsm.SetCurrentState((int)EnemyState.ATTACKSTATE);
            }

        }
    }



    
    /*
    - will move to a certain radius if the player is not in range of the attacking zone
    - will choose either laser, shotgun or rifle
    - shotgun will shot in three direction (left right and center of the player)
    - rifle will shot outward 
    - laser will shot randomly
     */

}