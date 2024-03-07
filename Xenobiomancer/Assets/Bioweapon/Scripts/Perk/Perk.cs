using Bioweapon;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Bioweapon
{

    public class PerkBase : ScriptableObject     
    {
        [Header("Perk details")]
        [SerializeField] private string nameOfPerk;
        [TextArea(2,3)]
        [SerializeField] private string description;
        [SerializeField] private int cost;
        [SerializeField] private Sprite logo;

        public string NameOfPerk { get => nameOfPerk; }
        public string Description { get => description; }
        public int Cost { get => cost; }
        public Sprite Logo { get => logo; }
    }


}