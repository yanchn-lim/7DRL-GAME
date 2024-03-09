using Patterns;
using System.Collections;
using UnityEngine;

namespace enemyT
{
    public class BasicAttackingEnemyState : BasicEnemyState
    {

        
        public BasicAttackingEnemyState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
            mId = (int)EnemyState.ATTACKSTATE;
        }



    }
}