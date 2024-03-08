using Patterns;
using UnityEngine;

namespace enemyT
{
    public class BasicChasingEnemyState : BasicEnemyState
    {
        
        public BasicChasingEnemyState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
            mId = (int)EnemyState.CHASING;
        }

        public override void Update()
        {
            Debug.Log("chasing State");
            RunTowardPlayer();
            if (FindPlayerWithinVision())
            {//just run in a straight line at it
            }
            else
            {
                //do some idle action
            }
        }

        private void RunTowardPlayer()
        {
            Vector2 direction = transform.position - playerReference.transform.position;
            transform.Translate(direction * enemyReference.Speed * Time.deltaTime);
            var targetRotation = Quaternion.FromToRotation(transform.up, direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, enemyReference.RotationSpeed * Time.deltaTime);
        }

    }

}