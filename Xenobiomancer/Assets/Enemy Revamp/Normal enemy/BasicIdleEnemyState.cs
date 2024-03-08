using Patterns;

namespace enemyT
{
    public class BasicIdleEnemyState : BasicEnemyState
    {

        public BasicIdleEnemyState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
            mId = (int)EnemyState.IDLE;
        }

        public override void Update()
        {
            if(FindPlayerWithinVision())
            {
                mFsm.SetCurrentState((int) EnemyState.CHASING);
            }
            else
            {
                //do some idle action
            }
        }

        //check if player is near enemy vision

    }

}