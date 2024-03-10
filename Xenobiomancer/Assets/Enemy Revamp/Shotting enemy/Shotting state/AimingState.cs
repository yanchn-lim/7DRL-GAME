using Bioweapon;
using enemyT;
using Patterns;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace enemyT
{
    public class AimingState : BasicEnemyState
    {
        private ShottingEnemy enemy;
        private bool canShoot;
        private float elapseTime;
        private float timeDecidedToShoot;
        private float minTimeShoot;
        private float maxTimeShoot;
        public AimingState(FSM fsm, ShottingEnemy enemy) : base(fsm, enemy)
        {
            this.enemy = enemy;
            minTimeShoot = enemy.MinTimeToShoot * GameManager.Instance.TurnTime;
            maxTimeShoot = enemy.MaxTimeToShoot * GameManager.Instance.TurnTime;
        }

        public override void Enter()
        {
            EventManager.Instance.AddListener(EventName.TURN_END, ActivatedShooting);
            
        }


        public override void Exit()
        {
            EventManager.Instance.RemoveListener(EventName.TURN_END, ActivatedShooting);
        }

        public override void Update()
        {
            Debug.Log("shooting state");
            base.Update();
            if (playerWithinVision)
            {
                Debug.Log("Player within range");

                if (canShoot)
                {
                    if (elapseTime > timeDecidedToShoot)
                    {
                        enemy.WeaponEquiped.FireBullet();
                        canShoot = false;
                    }
                    else 
                    {
                        elapseTime += Time.deltaTime;
                    }
                }
            }
            else if (playerWithinSenseRange)
            {
                RotateToFacePoint(playerReference.transform.position);
            }
        }

        private void ActivatedShooting() 
        {
            if (enemy.WeaponEquiped.CanShoot)
            {
                canShoot = true;
                timeDecidedToShoot = Random.Range(minTimeShoot, maxTimeShoot);
                elapseTime = 0f;
            }
            else
            {
                if (enemy.WeaponEquiped.CanReload)
                {
                    enemy.WeaponEquiped.ReloadFunction();
                }
            }
        }

        private void RotateToFacePoint(Vector2 targetPosition)
        {
            Vector2 direction = targetPosition - (Vector2)transform.position;

            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                var targetRotation = Quaternion.Euler(0f, 0f, angle);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyReference.RotationSpeed * Time.deltaTime);
            }
        }

    }
}