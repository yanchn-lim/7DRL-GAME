using Patterns;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace enemyT
{
    public class BasicChasingEnemyState : BasicEnemyState
    {
        //path represented as a path
        private Stack<Vector2> path;
        public BasicChasingEnemyState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
            mId = (int)EnemyState.CHASING;
        }
        private Vector2 currentPointToFollow;
        public override void Enter()
        {
            base.Enter();

            if (PlayerWithinSenseRange())
            { //will try to have one last attempt to find the player
                RotateToFacePoint(playerReference.transform.position);
            }
            if (PlayerWithinVision())
            {
                //get path
                GenerateNewPath();
            }
            else
            {
                path = enemyReference.Path;
            }

            if (path.Count > 0)
            {
                currentPointToFollow = path.Pop();
            }
            else
            {
                //go back to idling
                mFsm.SetCurrentState((int)EnemyState.IDLE);
            }
        }

        private void GenerateNewPath()
        {
            path = GridHelper.Instance.GeneratePath(transform.position, playerReference.transform.position);
            enemyReference.Path = path;
        }

        public override void Update()
        {
            //chasing logic
            if (PlayerWithinVision())
            {
                GenerateNewPath();
            }
            else if(PlayerWithinSenseRange() )
            {
                RotateToFacePoint(playerReference.transform.position);
            }
            MoveEnemyToPoint();
        }

        public override void Exit()
        {
            enemyReference.Path = path; //record it back to the enemy reference
            base.Exit();
        }

        private void MoveEnemyToPoint()
        {
            RotateToFacePoint(currentPointToFollow);

            float distanceBetweenPoint = Vector2.Distance(currentPointToFollow, transform.position); 
            if(distanceBetweenPoint <= enemyReference.PointSensingRadius)
            {
                if(path.Count > 0) //check if there is still a path
                {
                    currentPointToFollow = path.Pop();
                }
                else
                {//else return back to idling

                    mFsm.SetCurrentState((int)EnemyState.IDLE);
                    return;
                }
            }
            //move the transform position
            transform.position += transform.up * enemyReference.Speed * Time.deltaTime;


            //move the 
        }

        private void RotateToFacePoint(Vector2 targetPosition)
        {
            Vector2 direction = targetPosition - (Vector2) transform.position;

            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                var targetRotation = Quaternion.Euler(0f, 0f, angle);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyReference.RotationSpeed * Time.deltaTime);
            }
        }
    }

}