﻿using Patterns;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace enemyT
{
    public class BasicChasingEnemyState : BasicEnemyState
    {
        //path represented as a path
        public BasicChasingEnemyState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
            mId = (int)EnemyState.CHASING;
        }
        protected Vector2 currentPointToFollow;
        public override void Enter()
        {
            base.Enter();

            Debug.Log($"{enemyReference.name} can see player? {playerWithinVision}");
            if (playerWithinVision)
            {
                //get path
                Debug.Log($"{enemyReference.name}: seen player! generating path");
                GenerateNewPath();
                Debug.Log($"{enemyReference.name}: generated path");

            }
            else
            {
                //refer to old path
                Debug.Log($"{enemyReference.name}: did not see player! use old path");

                if (enemyReference.Path?.Count > 0)
                {
                    currentPointToFollow = enemyReference.Path.Pop();
                }
            }
        } 

        protected void GenerateNewPath()
        {
            enemyReference.Path = GridHelper.Instance.GeneratePath(transform.position, playerReference.transform.position);
            Debug.Log($"Generated {enemyReference.name} path. Path contains {enemyReference.Path.Count} node");
            currentPointToFollow = enemyReference.Path.Pop();
            
        }

        public override void Update()
        {
            base.Update();
            //chasing logic

            //generate new path every single time
            GenerateNewPath();

            if(playerWithinSenseRange )
            {
                RotateToFacePoint(playerReference.transform.position);
            }

            if(enemyReference.Path != null)
            {
                Debug.Log("Move enemy to point because path is not null");
                MoveEnemyToPoint();
                
            }
        }

        public override void Exit()
        {
            Debug.Log("Exiting");
            base.Exit();
        }

        protected void MoveEnemyToPoint()
        {
            RotateToFacePoint(currentPointToFollow);
            Debug.Log("Finish rotating the enemy to face player");

            float distanceBetweenPoint = Vector2.Distance(currentPointToFollow, transform.position);
            transform.position += transform.up * enemyReference.Speed * Time.deltaTime;
            Debug.Log("Finish moving  the enemy to face player");

            //this line of code
            if (distanceBetweenPoint <= enemyReference.PointSensingRadius)
            {
                Debug.Log("Distance between point is within enemy reach");
                if (enemyReference.Path?.Count > 0) //check if there is still a path
                {
                    Debug.Log("There is still a path so continue to pop and move to player");
                    currentPointToFollow = enemyReference.Path.Pop();
                }
                else
                {//else return back to idling

                    mFsm.SetCurrentState((int)EnemyState.IDLE);
                    return;
                }
            }
            //move the transform position
        }

        protected void RotateToFacePoint(Vector2 targetPosition)
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