using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bioweapon
{
    [CreateAssetMenu(fileName = "BioWeapnData", menuName = "ScriptableObjects/BioWeapnData", order = 1)]
    public class BioweaponScriptableObject : ScriptableObject
    {
        //for the gun
        [SerializeField] private int bulletFiredPerTurn;
        [SerializeField] private float bulletSpeedPerTurn;
        public int BulletFiredPerTurn { get => bulletFiredPerTurn; }
        public float BulletSpeedPerTurn { get => bulletSpeedPerTurn; }

        //for the bullets
        [SerializeField] private Sprite bullet;
        [SerializeField] private float bulletHitBoxRadius;
        [SerializeField] private int bulletKillTimer;
        public Sprite BulletSprite { get => bullet; }
        public float BulletHitBoxRadius { get => bulletHitBoxRadius; }

        /// <summary>
        /// how many turns the bullet before the bullet destroy itself.
        /// </summary>
        public int BulletKillTimer { get => bulletKillTimer; }


    }
}