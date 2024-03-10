using UnityEngine;

namespace Bioweapon
{
    [CreateAssetMenu(fileName = "shotgun perk", menuName = "ScriptableObjects/new perk/shotgun perk")]
    public class ShotgunPerk : PerkBase
    {
        [Header("Shotgun upgrades")]
        [Tooltip("reduce the shotgun spread of the bullet")]
        [SerializeField] private float reductionOfSpread;
        [Tooltip("increase the amount of shot")]
        [SerializeField] private int pelletIncrease;
        [Tooltip("increase the accuracy of the shot")]
        [SerializeField] private float accuracyIncrease;
        [Tooltip("increase the speed of the bullet")]
        [SerializeField] private float bulletSpeedIncrease;
        [Tooltip("long bullet lifetime")]
        [SerializeField] private int bulletLifeTimeIncrease;
        [Tooltip("increase max mag of the shotgun")]
        [SerializeField] private int increaseCurrentMag;
        [Tooltip("increase max ammo of the shotgun")]
        [SerializeField] private int increaseMaxAmmoOfTheMag;
        [SerializeField] private int damage;
        public int Damage { get => damage; }
        public float ReductionOfSpread { get => reductionOfSpread; }
        public int PelletIncrease { get => pelletIncrease; }
        public float AccuracyIncrease { get => accuracyIncrease; }
        public float BulletSpeedIncrease { get => bulletSpeedIncrease; }
        public int BulletLifeTimeIncrease { get => bulletLifeTimeIncrease; }
        public int IncreaseCurrentMag { get => increaseCurrentMag; }
        public int IncreaseMaxAmmoOfTheMag { get => increaseMaxAmmoOfTheMag; }
    }
}