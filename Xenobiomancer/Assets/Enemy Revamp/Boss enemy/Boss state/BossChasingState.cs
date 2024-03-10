using Patterns;
using System.IO;

using UnityEngine;

namespace enemyT
{
    public class BossChasingState : BasicChasingEnemyState
    {
        public BossChasingState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
        }

        public override void Enter()
        {
            EventManager.Instance.AddListener(EventName.TURN_COMPLETE, ReturnToIdleState);
            if (PlayerWithinSenseRange())
            { //will try to have one last attempt to find the player
                RotateToFacePoint(playerReference.transform.position);
            }
            GenerateNewPath();
            if (enemyReference.Path.Count > 0)
            {
                currentPointToFollow = enemyReference.Path.Pop();
            }
        }

        public override void Update()
        {
            //make sure that the boss is near to the player
            GenerateNewPath();
            MoveEnemyToPoint();
            RotateToFacePoint(playerReference.transform.position);
        }

    }
}