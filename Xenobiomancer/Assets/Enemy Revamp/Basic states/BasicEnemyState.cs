﻿using Patterns;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace enemyT
{
    public class BasicEnemyState : FSMState
    {
        /// <summary>
        /// enemy transform
        /// </summary>
        protected Transform transform;
        /// <summary>
        /// enemy class reference
        /// </summary>
        protected EnemyBase enemyReference;
        /// <summary>
        /// player reference
        /// </summary>
        protected Player playerReference;
        public BasicEnemyState(FSM fsm , EnemyBase enemy) : base(fsm)
        {
            enemyReference = enemy;
            transform = enemy.transform;
            playerReference = enemy.Player;
        }


        protected bool playerWithinVision { get; private set; }
        protected bool playerWithinSenseRange { get; private set; }
        //basic tool for the enemy
        protected bool PlayerWithinVision()
        {
            Vector2 playerPosition = playerReference.transform.position;
            float distanceBetweenEnemyAndPlayer = Vector2.Distance(playerPosition, transform.position);

            //rules to see if the player fall within the enemy vision
            if (distanceBetweenEnemyAndPlayer <= enemyReference.LengthOfVision)
            {//falls within the vision
                Debug.Log($"{enemyReference.name}: player within field of vision");
                Vector2 normalizePlayerPosition = (playerReference.transform.position - transform.position).normalized;
                float angleFromEnemyToPlayer = Vector2.Angle(transform.up, normalizePlayerPosition);
                //divide by two because up can only cover half of the degress vision
                if (angleFromEnemyToPlayer < enemyReference.DegreeOfVision / 2)
                {
                    Debug.Log($"{enemyReference.name}: player within the angle of vision");
                    //do raycast and see if the player is not being block by any walls


                    return true;
                    //Vector2 directionOfTheRay = playerPosition - (Vector2)transform.position;
                    //var hit = Physics2D.Raycast(
                    //    transform.position, 
                    //    directionOfTheRay, 
                    //    enemyReference.LengthOfVision,
                    //    1<<7  //the enemy laymask + wall layermask
                    //    );
                    //Debug.DrawRay(transform.position, directionOfTheRay , Color.red);

                    //if(hit.collider == null) 
                    //{
                    //    Debug.Log($"{enemyReference.name}: raycast did not hit");
                    //    return false;
                    //}
                    //else
                    //{
                    //    Debug.Log($"{enemyReference.name}: player raycast hit {hit.collider.name}");
                    //}
                    //if (hit.collider.gameObject.transform == playerReference.transform)
                    //{
                    //    //if same collider than it means it is in range
                    //    return true;
                    //}
                    
                }
            }

            //it cant find the player
            return false;

        }

        protected bool PlayerWithinSenseRange()
        {
            Vector2 playerPosition = playerReference.transform.position;
            float distance = Vector2.Distance(playerPosition, transform.position);
            if(distance <= enemyReference.PlayerSensingRadius)
            {
                return true;
            }
            return false;
        }

        public override void Enter()
        {
            //EventManager.Instance.AddListener(EventName.TURN_START, ReturnToIdleState);
            playerWithinVision = PlayerWithinVision();
            playerWithinSenseRange = PlayerWithinSenseRange();
            EventManager.Instance.AddListener(EventName.TURN_COMPLETE, ReturnToIdleState);
        }

        public override void Update()
        {
            playerWithinVision = PlayerWithinVision();
            playerWithinSenseRange = PlayerWithinSenseRange();
        }

        public override void Exit()
        {
            //EventManager.Instance.RemoveListener(EventName.TURN_START, ReturnToIdleState);
            EventManager.Instance.RemoveListener(EventName.TURN_COMPLETE, ReturnToIdleState);
        }

        protected void ReturnToIdleState()
        {
            mFsm.SetCurrentState((int)EnemyState.IDLE);
        }
    }

}