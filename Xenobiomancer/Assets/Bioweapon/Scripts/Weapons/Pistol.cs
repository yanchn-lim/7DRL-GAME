namespace Bioweapon
{
    public class Pistol : GunWeapon
    {
        public override void HideTrajectory()
        {
            throw new System.NotImplementedException();
        }
        public override void ShowTrajectory()
        {
            throw new System.NotImplementedException();
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