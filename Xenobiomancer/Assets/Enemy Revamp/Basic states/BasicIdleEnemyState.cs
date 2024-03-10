using enemySS;
using Patterns;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace enemyT
{
    public abstract class BasicIdleEnemyState : BasicEnemyState
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
            EventManager.Instance.RemoveListener(EventName.TURN_END, DecideNextState);
        }

        protected abstract void DecideNextState();
        //{
        //    Debug.Log($"Is there a path? {enemyReference.Path != null}");
        //    if (PlayerWithinVision() || enemyReference.Path != null || PlayerWithinSenseRange())
        //    {
        //        mFsm.SetCurrentState((int)EnemyState.CHASING);
        //    }
        //    else
        //    {
        //        mFsm.SetCurrentState((int)(EnemyState.IDLE));
        //    }
        //}
        public override void Update()
        {
            Debug.Log($"{enemyReference.name} is in: idle state");
            base.Update();
        }
    }

}