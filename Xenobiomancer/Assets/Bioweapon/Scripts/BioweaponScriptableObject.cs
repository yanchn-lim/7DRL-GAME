using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStation;

namespace Bioweapon
{
    [CreateAssetMenu(fileName = "BioWeapnData", menuName = "ScriptableObjects/BioWeapnData", order = 1)]
    public class BioweaponScriptableObject : ScriptableObject
    {
        //for the gun
        [Header("Gun information")]
        [Tooltip("How many bullet should be fired in one turn (make sure it is within the expiry of the start turn)")]
        [SerializeField] private int bulletFiredPerTurn;
        [Tooltip("Time taken to show each bullet if there is multiple bullet")]
        [SerializeField] private float bulletShowInterval;
        [Tooltip("How fast the bullet should move within that one turn")]
        [SerializeField] private float bulletSpeedPerTurn;

        #region public getter for gun information
        /// <summary>
        /// The time it takes to show each bullet when fired
        /// </summary>
        public float BulletSpawnInterval { get => bulletShowInterval;}
        /// <summary>
        /// How many bullet should be fired 
        /// </summary>
        public int BulletFiredPerTurn { get => bulletFiredPerTurn; }
        /// <summary>
        /// how fast the bullet should move in one turn
        /// </summary>
        public float BulletSpeedPerTurn { get => bulletSpeedPerTurn; }
        #endregion

        [Header("Bullet information")]
        //for the bullets
        [Tooltip("How the bullet should look like")]
        [SerializeField] private Sprite bullet;
        [Tooltip("bullet hitbox radius to sense any object")]
        [SerializeField] private float bulletHitBoxRadius;
        [Tooltip("How many turns before the bullet delete itself automatically if it does not hitanything")]
        [SerializeField] private int bulletKillTimer;

        [Range(0,1)]
        [Tooltip("Accuracy of the bullet")]
        [SerializeField] private float accuracy;
        [Range(0, 60)]
        [Tooltip("The offset of the bullet if it is not accurate")]
        [SerializeField] private float angleOfOffset;

        #region public getters for bullet
        /// <summary>
        /// bullet sprite to use
        /// </summary>
        public Sprite BulletSprite { get => bullet; }
        public float BulletHitBoxRadius { get => bulletHitBoxRadius; }

        /// <summary>
        /// how many turns the bullet before the bullet destroy itself.
        /// </summary>
        public int BulletKillTimer { get => bulletKillTimer; }
        /// <summary>
        /// How accurate the bullet can fire
        /// </summary>
        public float Accuracy { get => accuracy; }
        /// <summary>
        /// how the bullet will offset if the bullet is not accurate.
        /// </summary>
        public float AngleOfOffset { get => angleOfOffset; set => angleOfOffset = value; }
        #endregion


        public void AddPerk(Perk perk)
        {
            bulletFiredPerTurn += perk.BulletIncrease;
            //calculate how fast the bullet should fire to show all the bullet

            bulletShowInterval = GameManager.Instance.TurnTime / bulletFiredPerTurn;

            angleOfOffset -= perk.AngleOfOffsetReduction;
            bulletSpeedPerTurn += perk.BulletSpeedIncrease;

            bulletKillTimer += perk.IncreaseKillTimer;
            accuracy += perk.IncreaseAccuracy;
        }
    }
}