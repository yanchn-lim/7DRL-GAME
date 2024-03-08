using UnityEngine;

namespace Bioweapon
{
    [CreateAssetMenu(fileName = "Rifle perk", menuName = "ScriptableObjects/new perk/Rifle perk")]
    public class RiflePerk : PerkBase
    {
        [Header("Rifle upgrades")]
        [Tooltip("increase the amount of bullet shot")]
        [SerializeField] private int shotsIncrease;
        [Tooltip("increase the accuracy of the shot")]
        [Range(0,1)]
        [SerializeField] private float accuracyIncrease;
        [Tooltip("Decrease the spread of the bullet")]
        [Range(0, 40)]
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

        
    }
}