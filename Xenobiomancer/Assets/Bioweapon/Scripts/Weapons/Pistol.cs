namespace Bioweapon
{
    public class Pistol : GunWeapon
    {
        protected override void MethodToFireBullet()
        {
            var bullet = poolOfBullet.Get();
            bullet.transform.position = firingPosition.position;
            bullet.transform.rotation = gun.rotation;
            bullet.FireBullet();

        }
    }
}