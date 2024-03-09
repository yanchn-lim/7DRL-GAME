using Patterns;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace enemyT
{
    public class BasicChasingEnemyState : BasicEnemyState
    {
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
            //path = GridHelper.Instance.GeneratePath(transform.position, playerReference.transform.position);
            //enemyReference.Path = path;


            if (path.Count > 0)
            {
                currentPointToFollow = path.Pop();
            }
            else
            {
                Debug.Log("return back to idle");
                //go back to idling
                mFsm.SetCurrentState((int)EnemyState.IDLE);
            }
            /*ShowDebugPath();*/

        }

        private void GenerateNewPath()
        {
            path = GridHelper.Instance.GeneratePath(transform.position, playerReference.transform.position);
            enemyReference.Path = path;
        }

        public override void Update()
        {
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
            Debug.Log($"Node need to travel for current path?: {path.Count}"); //why is it being exited ever single time
            //path.Push(currentPointToFollow);
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
                    Debug.Log($"pop from stack! Now left with: {path.Count}");
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

        #region legacy 
        //private void ShowDebugPath()
        //{
        //    if(path.Count == 0) 
        //    {
        //        Debug.Log("no path");
        //        return; 
        //    }
        //    Debug.Log($"current there are {path.Count}");
        //    Stack<Vector2> debugDummy = new Stack<Vector2>(path);
        //    Vector2 previous = debugDummy.Pop();
        //    while (debugDummy.Count > 0)
        //    {
        //        Vector2 position = debugDummy.Pop();
        //        Debug.DrawLine(previous, position, UnityEngine.Color.yellow , 1f);
        //        previous = position;
        //    }
        //}

        //this section is working
        //private void RunTowardPlayer()
        //{
        //    Vector2 direction = playerReference.transform.position - transform.position ;

        //    if (direction != Vector2.zero)
        //    {
        //        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        //        //* enemyReference.RotationSpeed * Time.de
        //        var targetRotation = Quaternion.Euler(0f, 0f, angle);
        //        transform.rotation = Quaternion.Slerp(transform.rotation , targetRotation , enemyReference.RotationSpeed * Time.deltaTime);
        //    }
        //    Debug.DrawLine(transform.position, transform.position + transform.up , Color.yellow);
        //    transform.Translate(transform.up * enemyReference.Speed * Time.deltaTime);
        //}
        #endregion
        private void RotateToFacePoint(Vector2 targetPosition)
        {
            Vector2 direction = targetPosition - (Vector2) transform.position;

            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                //* enemyReference.RotationSpeed * Time.de
                var targetRotation = Quaternion.Euler(0f, 0f, angle);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyReference.RotationSpeed * Time.deltaTime);
            }
        }



    }

}