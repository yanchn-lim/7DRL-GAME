using Patterns;
using System;
using System.Collections;
using Unity.VisualScripting;
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
            int current = (int)previousState;
            current++;
            if(current >= Enum.GetNames(typeof(AttackingState)).Length)
            {
                current = 0;
            }
            AttackingState currentAttackingState = (AttackingState)current;

            //decide on the next thing to do
            switch(currentAttackingState)
            {
                case (AttackingState.LASER):
                    AttackWithLaser();
                    break;
                case (AttackingState.RIFLE):
                    AttackWithRifle();
                    break;
                case(AttackingState.SHOTGUN): 
                    AttackWithShotgun();
                    break;
                case (AttackingState.ALL):
                    AttackWithLaser();
                    AttackWithRifle();
                    AttackWithShotgun();
                    break;
                default:
                    break;
            }
            previousState = currentAttackingState;
        }


        #region laser related
        private void AttackWithLaser()
        {
            Vector2 targetPosition;
            
            if(UnityEngine.Random.value < enemy.LaserPinPointAccuracy)
            {
                targetPosition = playerReference.transform.position;
            }
            else
            {
                float angleOfInaccuracy = UnityEngine.Random.Range(-enemy.LaserAngleOfInaccuracy, enemy.LaserAngleOfInaccuracy);
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

        #region rilfe related

        private void AttackWithRifle()
        {
            enemy.PrepareFireRifleCoroutine(playerReference.transform.position);
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

        private enum AttackingState
        {
            LASER = 0,
            RIFLE,
            SHOTGUN,
            ALL,
            RELOAD
        }

    }
}