using Patterns;
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
        public bool FindPlayerWithinVision()
        {
            Vector2 playerPosition = playerReference.transform.position;
            float distanceBetweenEnemyAndPlayer = Vector2.Distance(playerPosition, transform.position);

            //rules to see if the player fall within the enemy vision
            if (distanceBetweenEnemyAndPlayer <= enemyReference.LengthOfVision)
            {//falls within the vision
                float angleFromEnemyToPlayer = Vector2.Angle(transform.up, playerPosition);

                //divide by two because up can only cover half of the degress vision
                if (angleFromEnemyToPlayer < enemyReference.DegreeOfVision / 2)
                {
                    //do raycast and see if the player is not being block by any walls
                    Vector2 directionOfTheRay = playerPosition - (Vector2)transform.position;
                    var hit = Physics2D.Raycast(transform.position, directionOfTheRay, enemyReference.LengthOfVision);

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
    }

}