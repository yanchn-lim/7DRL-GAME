using UnityEngine;
using UnityEngine.UI;
using UpgradeStation;

namespace Bioweapon
{
    public class Shotgun: GunWeapon
    {
        [Header("Shotgun detail")]
        [SerializeField] private Image shootingTrajectory;
        [SerializeField] private float angleOfSpread;

        private const float fillAngleRatio = 1f / 360f;

        protected override void Start()
        {
            base.Start();
            AdjustingTrjectory();
        }

        private void AdjustingTrjectory()
        {
            shootingTrajectory.rectTransform.rotation = Quaternion.Euler(0f, 0f, angleOfSpread / 2);
            shootingTrajectory.fillAmount = angleOfSpread * fillAngleRatio;
        }

        protected override void MethodToFireBullet()
        {
            for(int i = 0; i < bulletFiredPerTurn; i++)
            {
                var bullet = poolOfBullet.Get();
                bullet.transform.position = firingPosition.position;
                float randomSpreadAngle = Random.Range(-angleOfSpread/2 , angleOfSpread/2);
                bullet.transform.rotation = gun.rotation * Quaternion.AngleAxis(randomSpreadAngle , Vector3.forward);
                bullet.FireBullet();
            }
        }

        public override void ShowTrajectory()
        {
            shootingTrajectory.gameObject.SetActive(true);
        }

        public override void HideTrajectory()
        {
            shootingTrajectory.gameObject.SetActive(false);
        }

        private void UpgradeShotGun(ShotgunPerk perk)
        {
            angleOfSpread -= perk.ReductionOfSpread;
            bulletFiredPerTurn += perk.PelletIncrease;
            accuracy += perk.AccuracyIncrease;
            bulletSpeedPerTurn += perk.BulletSpeedIncrease;
            bulletKillTimer += perk.BulletLifeTimeIncrease;
            maxMagSize += perk.IncreaseMaxAmmoOfTheMag;
            ammoIncrease += perk.IncreaseMaxAmmoOfTheMag;

        }

        public override void Upgrade(int i)
        {
            ShotgunPerk perk = upgradeData.ShotgunPerks[i];
            UpgradeShotGun(perk);
            perkGunGain.Add(perk);
        }
    }
}