using Patterns;
using System.Collections;
using UnityEngine;

namespace enemyT
{
    public class NormalEnemyIdleState : BasicIdleEnemyState
    {
        private Animator animator;

        public Animator Animator { get { return animator; } }
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

        public override void Update()
        {
            animator.SetFloat("Pos X", 0);
            animator.SetFloat("Pos Y", 0);
            base.Update();
        }
    }
}