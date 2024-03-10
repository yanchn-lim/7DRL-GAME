using System.Collections;
using UnityEngine;

namespace Bioweapon
{
    public class Rifle : GunWeapon
    {
        [Header("Rifle variable")]
        [SerializeField] private GameObject trajectory;

        public override void HideTrajectory()
        {
            trajectory.SetActive(false);
        }

        public override void ShowTrajectory()
        {
            trajectory.SetActive(true);
        }


        protected override void MethodToFireBullet()
        {
            StartCoroutine(ShootingCoroutine());
        }

        private IEnumerator ShootingCoroutine()
        {

            float intervalTime = GameManager.Instance.TurnTime / bulletFiredPerTurn;
            for(int i = 0; i < bulletFiredPerTurn; i++)
            {
                var bullet = poolOfBullet.Get();
                bullet.transform.position = firingPosition.position;
                bullet.transform.rotation = gun.rotation;
                bullet.FireBullet();
                yield return new WaitForSeconds(intervalTime);
            }
        }

        public void UpgradeStats(RiflePerk perk)
        {
            bulletFiredPerTurn += perk.ShotsIncrease;
            accuracy += perk.AccuracyIncrease;
            angleOfOffset -= perk.ReduceBulletSpread;
            bulletSpeedPerTurn += perk.BulletSpeedIncrease;
            bulletKillTimer += perk.BulletLifeTimeIncrease;
            maxMagSize += perk.IncreaseMaxAmmo;
            bulletDamage += perk.Damage;
        }

        public override void Upgrade(int i)
        {
            RiflePerk perk = upgradeData.RiflePerks[i];
            UpgradeStats(perk);
            perkGunGain.Add(perk);
        }
    }

}