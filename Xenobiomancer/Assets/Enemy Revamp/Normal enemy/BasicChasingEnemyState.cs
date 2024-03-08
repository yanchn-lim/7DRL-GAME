using Patterns;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace enemyT
{
    public class BasicChasingEnemyState : BasicEnemyState
    {
        Stack<Vector2> path;
        public BasicChasingEnemyState(FSM fsm, EnemyBase enemy) : base(fsm, enemy)
        {
            mId = (int)EnemyState.CHASING;
        }

        public override void Update()
        {
            if (PlayerWithinVision())
            {
                path = GridHelper.Instance.GeneratePath(transform.position, playerReference.transform.position);
                //get path
                RotateToPlayer();
            }
            
        }

        private void RotateToPlayer()
        {
            Vector2 direction = playerReference.transform.position - transform.position;

            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                //* enemyReference.RotationSpeed * Time.de
                var targetRotation = Quaternion.Euler(0f, 0f, angle);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyReference.RotationSpeed * Time.deltaTime);
            }
        }

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

    }

}