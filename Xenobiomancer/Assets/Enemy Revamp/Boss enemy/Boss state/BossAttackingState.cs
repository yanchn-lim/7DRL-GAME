using Patterns;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace enemyT
{
    public class BossAttackingState : BasicEnemyState
    {
        private BossEnemy enemy;
        private AttackingState previousState;
        public BossAttackingState(FSM fsm, BossEnemy enemy) : base(fsm, enemy)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            //AttackWithLaser();
            AttackWithShotgun();
        }


        #region laser related
        private void AttackWithLaser()
        {
            Vector2 targetPosition;
            
            if(Random.value < enemy.LaserPinPointAccuracy)
            {
                targetPosition = playerReference.transform.position;
            }
            else
            {
                float angleOfInaccuracy = Random.Range(-enemy.LaserAngleOfInaccuracy, enemy.LaserAngleOfInaccuracy);
                targetPosition = RotatePoint(playerReference.transform.position, transform.position, angleOfInaccuracy);
            }
            enemy.PrepareFireLaserCoroutine(targetPosition);

        }


        //private void GetRandomLaserPosition
        #endregion

        #region Shotgun related
        private void AttackWithShotgun()
        {
            Vector2 centrePoint = playerReference.transform.position;
            enemy.PrepareFireShotgunCoroutine(centrePoint);
        }


        #endregion
        Vector2 RotatePoint(Vector2 point, Vector2 center, float angle)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);

            // Translate the point back to the origin
            float x = point.x - center.x;
            float y = point.y - center.y;

            // Rotate the point
            float xNew = x * cos - y * sin;
            float yNew = x * sin + y * cos;

            // Translate the point back to its original position
            xNew += center.x;
            yNew += center.y;

            return new Vector2(xNew, yNew);
        }

        //protected void RotateToFacePoint(Vector2 targetPosition)
        //{
        //    Vector2 direction = targetPosition - (Vector2)transform.position;

        //    if (direction != Vector2.zero)
        //    {
        //        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        //        var targetRotation = Quaternion.Euler(0f, 0f, angle);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyReference.RotationSpeed * Time.deltaTime);
        //    }
        //}

        private enum AttackingState
        {
            LASER = 0,
            SHOTGUN,
            RIFLE,
            ALL
        }

    }
}