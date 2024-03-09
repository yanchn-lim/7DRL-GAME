using enemyT;
using Patterns;
using System.Collections;
using UnityEngine;

namespace enemyT
{
    public class BasicDeathState : BasicEnemyState
    {
        public BasicDeathState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
        }

        public override void Enter()
        {
            //remove all the listener in the state and delete the game object//
            GameObject.Destroy(enemyReference.gameObject);
        }
        public override void Exit() 
        { }
    }
}