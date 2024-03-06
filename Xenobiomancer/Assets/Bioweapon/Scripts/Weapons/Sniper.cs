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


        protected override void MethodToFireBullet()
        {
            var bullet = poolOfBullet.Get();
            bullet.transform.position = firingPosition.position;
            bullet.transform.rotation = gun.rotation;
            bullet.FireBullet();
        }

    }

}