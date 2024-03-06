using Patterns;
using System;
using UnityEngine;

namespace Bioweapon
{
    public abstract class GunWeapon : Weapon
    {
        #region bullet
        [Header("Bullet information")]
        //for the bullets
        [Tooltip("How the bullet should look like")]
        [SerializeField] protected Sprite bullet;
        [Tooltip("bullet hitbox radius to sense any object")]
        [SerializeField] protected float bulletHitBoxRadius;
        [Tooltip("How many turns before the bullet delete itself automatically if it does not hitanything")]
        [SerializeField] protected int bulletKillTimer;
        [Range(0, 1)]
        [Tooltip("Accuracy of the bullet")]
        [SerializeField] protected float accuracy;
        [Range(0, 60)]
        [Tooltip("The offset of the bullet if it is not accurate")]
        [SerializeField] protected float angleOfOffset;
        #endregion

        #region public get for bullet
        public Sprite BulletSprite { get => bullet; }
        public float BulletHitBoxRadius { get => bulletHitBoxRadius; }
        public int BulletKillTimer { get => bulletKillTimer; }
        public float Accuracy { get => accuracy; }
        public float AngleOfOffset { get => angleOfOffset; }
        #endregion

        #region gun related
        [Tooltip("How many bullet should be fired in one turn (make sure it is within the expiry of the start turn)")]
        [SerializeField] protected int bulletFiredPerTurn;
        [Tooltip("Time taken to show each bullet if there is multiple bullet")]
        [SerializeField] protected float bulletShowInterval;
        [Tooltip("How fast the bullet should move within that one turn")]
        [SerializeField] protected float bulletSpeedPerTurn;
        #endregion

        #region public getter gun
        /// <summary>
        /// The time it takes to show each bullet when fired
        /// </summary>
        public float BulletSpawnInterval { get => bulletShowInterval; }
        /// <summary>
        /// How many bullet should be fired 
        /// </summary>
        public int BulletFiredPerTurn { get => bulletFiredPerTurn; }
        /// <summary>
        /// how fast the bullet should move in one turn
        /// </summary>
        public float BulletSpeedPerTurn { get => bulletSpeedPerTurn; }
        #endregion

        #region pool
        //bullet related
        [SerializeField] protected Transform poolContainer; // the pool container of bullet the player will have
        [SerializeField] protected Bullet bulletPrefab; //The prefab of the bullet to use when player fire
        protected PoolingPattern<Bullet> poolOfBullet; // the pool that will be used
        #endregion


        protected override void Start()
        {
            base.Start();
            SetUpBullet(); //set up the pool 
            EventManager.Instance.AddListener(EventName.TURN_COMPLETE, (Action)StopFiringBulletOnTurnComplete); //make sure the bullets wont fire after the turn is completed
        }

        //make sure to stop the coroutine after the turn is completed
        private void StopFiringBulletOnTurnComplete()
        {
            StopAllCoroutines();
        }

        #region bullet related
        private void SetUpBullet()
        {
            poolOfBullet = new PoolingPattern<Bullet>(bulletPrefab.gameObject);
            poolOfBullet.InitWithParent(10, poolContainer, InitCommand);
        }

        private Bullet InitCommand(Bullet bulletComponent)
        {
            bulletComponent.Init(this);
            return bulletComponent;
        }

        #endregion

        public void ReturnBullet(Bullet bullet)
        {
            poolOfBullet.Retrieve(bullet);
        }
    }
}