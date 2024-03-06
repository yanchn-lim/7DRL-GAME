using Bioweapon;
using System.Collections;
using UnityEngine;

namespace Bioweapon
{

    public class PerkBase : ScriptableObject     
    {
        [Header("Perk details")]
        [SerializeField] private string nameOfPerk;
        [TextArea(2,3)]
        [SerializeField] private string description;

    }

    public abstract class Perk<weaponType>: PerkBase where weaponType : Weapon
    {
        public abstract void Upgrade(weaponType weapon);
    }


}