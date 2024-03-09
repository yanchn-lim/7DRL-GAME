using Bioweapon;
using enemyT;
using Patterns;
using System.Collections;
using UnityEngine;

namespace enemyT
{
    public class ShottingEnemy : EnemyBase
    {
        [Tooltip("weapon type that the monster can use")]
        [SerializeField] private GunWeapon weaponEquiped;
        [Tooltip("min time to shoot (in turn time)")]
        [Range(0, 1)]
        [SerializeField] private float minTimeToShoot;
        [Tooltip("max time to shoot (in turn time)")]
        [Range(0,1)]
        [SerializeField] private float maxTimeToShoot;
        public Weapon WeaponEquiped { get => weaponEquiped; }
        public float MinTimeToShoot { get => minTimeToShoot; }
        public float MaxTimeToShoot { get => maxTimeToShoot; }

        protected override void SetupFSM()
        {
            fsm = new FSM();
            fsm.Add((int)EnemyState.SHOOTINGSTATE, new AimingState(fsm, this));
            fsm.Add((int)EnemyState.DEATHSTATE, new BasicDeathState(fsm, this));
            fsm.SetCurrentState((int)EnemyState.SHOOTINGSTATE);
        }

        protected override void StartDeath()
        {
            weaponEquiped.RemoveListener();
            fsm.SetCurrentState((int)EnemyState.DEATHSTATE);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, playerSensingRadius);
        }
    }
}