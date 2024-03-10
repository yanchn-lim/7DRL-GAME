using UnityEngine;

namespace Bioweapon
{
    public class Sniper : GunWeapon
    {
        [Header("Sniper variable")]
        [SerializeField] private GameObject trajectory;

        public override void HideTrajectory()
        {
            trajectory.SetActive(false);
        }

        public override void ShowTrajectory()
        {
            trajectory.SetActive(true);
        }

        public void UpgradeStats(SniperPerk perk)
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
            SniperPerk perk = upgradeData.SniperPerks[i];
            UpgradeStats(perk);
            perkGunGain.Add(perk);
        }

        protected override void MethodToFireBullet()
        {
            var bullet = poolOfBullet.Get();
            bullet.transform.position = firingPosition.position;
            bullet.transform.rotation = gun.rotation;
            bullet.FireBullet();
        }

    }

}