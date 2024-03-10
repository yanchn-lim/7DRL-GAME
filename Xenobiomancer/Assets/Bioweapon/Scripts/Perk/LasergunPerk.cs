using UnityEngine;

namespace Bioweapon
{
    [CreateAssetMenu(fileName = "lasergun perk", menuName = "ScriptableObjects/new perk/lasergun perk")]
    public class LasergunPerk : PerkBase
    {
        [Tooltip("Increase the width of the laser")]
        [SerializeField]private float increaseBeamWidth;
        [Tooltip("Increase the length of the laser")]
        [SerializeField] private float increaseBeamLength;
        [Tooltip("reduce the charging up of the beam")]
        [SerializeField] private float reduceChargeUp;
        [SerializeField] private int increaseMaxAmmo;
        [Tooltip("Damage increase")]
        [SerializeField] private int damage;
        public int Damage { get => damage; }

        public float IncreaseBeamWidth { get => increaseBeamWidth; }
        public float IncreaseBeamLength { get => increaseBeamLength; }
        public float ReduceChargeUp { get => reduceChargeUp; }
        public int IncreaseMaxAmmo { get => increaseMaxAmmo; }
    }
}