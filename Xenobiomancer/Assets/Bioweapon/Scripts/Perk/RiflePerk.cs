using UnityEngine;

namespace Bioweapon
{
    [CreateAssetMenu(fileName = "shotgun perk", menuName = "ScriptableObjects/new perk/shotgun perk")]
    public class RiflePerk : Perk<Rifle>
    {
        [Header("Rifle upgrades")]
        [Tooltip("increase the amount of bullet shot")]
        [SerializeField] private int shotsIncrease;
        [Tooltip("increase the accuracy of the shot")]
        [SerializeField] private float accuracyIncrease;
        [Tooltip("Decrease the spread of the bullet")]
        [SerializeField] private float reduceBulletSpread;
        [Tooltip("increase the speed of the bullet")]
        [SerializeField] private float bulletSpeedIncrease;
        [Tooltip("long bullet lifetime")]
        [SerializeField] private int bulletLifeTimeIncrease;
        [Tooltip("Increase max ammo")]
        [SerializeField] private int increaseMaxAmmo;

        public int ShotsIncrease { get => shotsIncrease; }
        public float AccuracyIncrease { get => accuracyIncrease; }
        public float ReduceBulletSpread { get => reduceBulletSpread; }
        public float BulletSpeedIncrease { get => bulletSpeedIncrease; }
        public int BulletLifeTimeIncrease { get => bulletLifeTimeIncrease; }
        public int IncreaseMaxAmmo { get => increaseMaxAmmo; }

        public override void Upgrade(Rifle weapon)
        {
            weapon.AddPerk(this);
            weapon.Upgrade(this);
        }
    }
}