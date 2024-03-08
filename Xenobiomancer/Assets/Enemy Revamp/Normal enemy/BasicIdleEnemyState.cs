using Patterns;
using UnityEngine;

namespace enemyT
{
    public class BasicIdleEnemyState : BasicEnemyState
    {

        public BasicIdleEnemyState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
            mId = (int)EnemyState.IDLE;
        }

        public override void Enter()
        {
            EventManager.Instance.AddListener(EventName.TURN_END, DecideNextState);
        }
        public override void Exit() 
        {
            EventManager.Instance.AddListener(EventName.TURN_END, DecideNextState);
        }

        private void DecideNextState()
        {

            if (PlayerWithinVision() || enemyReference.Path != null)
            {
                mFsm.SetCurrentState((int)EnemyState.CHASING);
            }
            else
            {
                mFsm.SetCurrentState((int)(EnemyState.IDLE));
            }
        }
        public override void Update()
        {
            Debug.Log("idle State");
        }
        //    if (PlayerWithinVision())
        //    {
        //        mFsm.SetCurrentState((int) EnemyState.CHASING);
        //    }
        //    else
        //    {
        //        //do some idle action
        //    }
        //}


        }

}