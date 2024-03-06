using Bioweapon;
using UnityEngine;

namespace UpgradeStation
{
    [CreateAssetMenu(fileName = "Perk", menuName = "ScriptableObjects/Perk")]
    public class Perk : ScriptableObject
    {
        //name of the perk
        [Header("Perk detail")]
        [Tooltip("what should the main name of the perk be?")]
        [SerializeField] private string nameOfThePerk;
        [Tooltip("What should the description of the perk be?")]
        [TextArea(2, 3)]
        [SerializeField] private string shortDescription;


        //for the gun
        [Header("Gun information")]
        [Tooltip("How many bullet should be fired in one turn")]//calculation will be done 
        [SerializeField] private int bulletIncrease;
        [Tooltip("How fast the bullet should move within that one turn")]
        [SerializeField] private float bulletSpeedIncrease;

        //bullet
        [Range(0, 1)]
        [Tooltip("increase the accuracy of the gun")]
        [SerializeField] private float increaseAccuracy;
        [Range(0, 50)]
        [Tooltip("reduce the  spread of the bullets")]
        [SerializeField] private float angleOfOffsetReduction;
        [Tooltip("Increase the bullet killer timer")]
        [SerializeField] private int increaseKillTimer;

        /// <summary>
        /// How much the bullet should increase when fired per turn
        /// </summary>
        public int BulletIncrease { get => bulletIncrease; }
        /// <summary>
        /// How fast the bullet should move
        /// </summary>
        public float BulletSpeedIncrease { get => bulletSpeedIncrease; }
        /// <summary>
        /// How much accuracy the bullet are at hitting the target
        /// </summary>
        public float IncreaseAccuracy { get => increaseAccuracy; }
        /// <summary>
        /// how much the spread should be reduce
        /// </summary>
        public float AngleOfOffsetReduction { get => angleOfOffsetReduction; }
        /// <summary>
        /// increase the kill timer of the bullet
        /// </summary>
        public int IncreaseKillTimer { get => increaseKillTimer; }
    }
}