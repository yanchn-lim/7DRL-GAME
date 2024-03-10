using enemyT;
using Patterns;
using System.Collections;
using UnityEngine;

namespace enemyT
{
    public class NormalEnemyChasingState : BasicChasingEnemyState
    {
        private NormalEnemy enemy;
        public NormalEnemyChasingState(FSM fsm, NormalEnemy enemy) : base(fsm, enemy)
        {
            this.enemy = enemy;
            mId = (int)EnemyState.CHASING;
        }

        public override void Update()
        {
            base.Update();
            CheckIfPlayerIsNearForAttacking();
        }

        private void CheckIfPlayerIsNearForAttacking()
        {
            if (!playerWithinVision) return;
            float distance = Vector2.Distance(playerReference.transform.position, enemy.transform.position);
            if (distance < enemy.DamageRadius) 
            {
                mFsm.SetCurrentState((int) EnemyState.ATTACKSTATE);
            }
        }

    }
}