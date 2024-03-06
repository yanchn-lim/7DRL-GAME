using UnityEngine;

namespace Bioweapon
{
    [CreateAssetMenu(fileName = "shotgun perk", menuName = "ScriptableObjects/new perk/shotgun perk")]
    public class ShotgunPerk : Perk<Shotgun>
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
        [SerializeField] private float bulletLifeTimeIncrease;
       
        public override void Upgrade(Shotgun weapon)
        {
            weapon.AddPerk(this);
            

        }
    }
}