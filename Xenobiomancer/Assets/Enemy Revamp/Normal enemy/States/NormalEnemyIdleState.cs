using Patterns;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace enemyT
{
    public class NormalEnemyIdleState : BasicIdleEnemyState
    {
        protected Animator _animator {get{ return NormalEnemy.FindAnyObjectByType<Animator>(); } }
        
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
            _animator.SetFloat("PosX", 0);
            _animator.SetFloat("PosY", 0);
        }
    }
}