using UnityEngine;

namespace Bioweapon
{
    [CreateAssetMenu(fileName = "lasergun perk", menuName = "ScriptableObjects/new perk/lasergun perk")]
    public class LasergunPerk : PerkBase
    {
        [Tooltip("Increase the width of the laser")]
        private float increaseBeamWidth;
        [Tooltip("Increase the length of the laser")]
        private float increaseBeamLength;
        [Tooltip("reduce the charging up of the beam")]
        private float reduceChargeUp;


        public float IncreaseBeamWidth { get => increaseBeamWidth; }
        public float IncreaseBeamLength { get => increaseBeamLength; }
        public float ReduceChargeUp { get => reduceChargeUp; }
    }
}