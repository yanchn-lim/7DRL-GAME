using Unity.Mathematics;
using UnityEngine;

namespace Bioweapon
{
    public class Pistol : GunWeapon
    {
        [Header("Pistol variable")]
        [SerializeField] private GameObject trajectory;

 

        public override void HideTrajectory()
        {
            trajectory.SetActive(false);
        }

        public override void ShowTrajectory()
        {
            trajectory.SetActive(true);
        }

        public override void Upgrade(int i)
        {
            print("no upgrades");
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