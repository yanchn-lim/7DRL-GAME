using Patterns;
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

        //basic tool for the enemy
        public bool PlayerWithinVision()
        {
            Vector2 playerPosition = playerReference.transform.position;
            float distanceBetweenEnemyAndPlayer = Vector2.Distance(playerPosition, transform.position);

            //rules to see if the player fall within the enemy vision
            if (distanceBetweenEnemyAndPlayer <= enemyReference.LengthOfVision)
            {//falls within the vision
                Vector2 normalizePlayerPosition = (playerReference.transform.position - transform.position).normalized;
                float angleFromEnemyToPlayer = Vector2.Angle(transform.up, normalizePlayerPosition);
                //divide by two because up can only cover half of the degress vision
                if (angleFromEnemyToPlayer < enemyReference.DegreeOfVision / 2)
                {

                    //do raycast and see if the player is not being block by any walls
                    Vector2 directionOfTheRay = playerPosition - (Vector2)transform.position;
                    var hit = Physics2D.Raycast(
                        transform.position, 
                        directionOfTheRay, 
                        enemyReference.LengthOfVision,
                        ~(1<<6) //the enemy laymask
                        );

                    if(hit.collider == null) 
                    {
                        return false;
                    }
                    if (hit.collider.gameObject.transform == playerReference.transform)
                    {
                        //if same collider than it means it is in range
                        return true;
                    }
                }
            }

            //it cant find the player
            return false;

        }

        public bool PlayerWithinSenseRange()
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
            EventManager.Instance.AddListener(EventName.TURN_COMPLETE, ReturnToIdleState);
        }

        public override void Exit()
        {
            //EventManager.Instance.RemoveListener(EventName.TURN_START, ReturnToIdleState);
            EventManager.Instance.RemoveListener(EventName.TURN_COMPLETE, ReturnToIdleState);
        }

        private void ReturnToIdleState()
        {
            mFsm.SetCurrentState((int)EnemyState.IDLE);
        }
    }

}