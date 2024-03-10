using Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;
using UpgradeStation;

namespace Bioweapon
{
    public abstract class Weapon : MonoBehaviour   
    {
        //bullet related

        [SerializeField] protected Transform gun; //the gun the play is using
        [SerializeField] protected Transform firingPosition; //where the bullets will be fired at
        [SerializeField] protected UpgradeStationData upgradeData;

        #region generic gun function

        [Tooltip("what is the gun type")]
        [SerializeField] protected GunType gunType;
        [Tooltip("what should be the name of the weapon")]
        [SerializeField] protected string nameOfTheWeapon;
        [Tooltip("cost of the weapon")]
        [SerializeField] protected int cost;
        [Tooltip("short description of the gun")]
        [SerializeField] protected string shortDescription;
        [Tooltip("max mag size")]
        [SerializeField] protected int maxMagSize;
        [Tooltip("current mag size")]
        [SerializeField] protected int currentMagSize;
        [Tooltip("Current Ammo")]
        [SerializeField] protected int ammoSize;
        [Tooltip("Ammo increase if they plan to buy more ammo")]
        [SerializeField] protected int ammoIncrease;

        [Tooltip("The counter to count how many turn it took to reload the gun")]
        [SerializeField] protected int reloadCounter;
        [Tooltip("How many turn should the reload take?")]
        [SerializeField] protected int reloadTurn;
        public bool CanShoot { get { return currentMagSize > 0; } }
        public bool CanReload { get { return ammoSize > 0; } }
        public bool HaveReloaded { get; private set; }
        public int ReloadCounter { get => reloadCounter; }
        public int ReloadTurn { get => reloadTurn; }
        public string NameOfTheWeapon { get => nameOfTheWeapon; }
        public int Cost { get => cost; }
        public GunType GunType { get => gunType; }
        protected List<PerkBase> perkGunGain = new List<PerkBase>();
        public List<PerkBase> PerkGunGain { get => perkGunGain; }
        public Transform FiringPosition { get => firingPosition; }

        #endregion


        protected virtual void Start()
        {
            //this is to prevent any missing variable that the you might miss
            if (gun == null) Debug.LogError("No gun attact!");
            if (firingPosition == null) Debug.LogError("No firing position indicated");
        }


        public virtual void UpdateFunction()
        {
            RotateGunBasedOnMousePosition();
            if (Input.GetKeyUp(KeyCode.Space))
            {
                EventManager.Instance.TriggerEvent(EventName.TURN_END);
                FireBullet();
            }
        }

        private void RotateGunBasedOnMousePosition()
        {
            // Get the position of the mouse cursor in world coordinates
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; // Make sure z position is zero for 2D

            RotateGunBasedOnPosition(mousePos);
        }

        public void RotateGunBasedOnPosition(Vector3 TargetPosition)
        {
            // Calculate direction from the object to the mouse cursor
            Vector3 direction = TargetPosition - transform.position;

            // Calculate the angle from the current forward direction to the direction to the cursor
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the object towards the cursor
            float eulerZAngle = Quaternion.AngleAxis(angle, Vector3.forward).eulerAngles.z;

            //quad 1: 0 -90
            //quad 2: 90-180
            //quad 3: 180-270 flip to y 180 and z
            //quad 4: 270-360
            //deciding the direction the gun should face
            if ((0f <= eulerZAngle && eulerZAngle <= 90f) || (270f <= eulerZAngle && eulerZAngle <= 360f))
            {
                gun.rotation = Quaternion.Euler(0, 0, eulerZAngle);
            }
            else
            {
                eulerZAngle = -(eulerZAngle - 180);
                gun.rotation = Quaternion.Euler(0, 180, eulerZAngle);
            }
        }

        /// <summary>
        /// how the gun would fire the bullet
        /// </summary>
        protected abstract void MethodToFireBullet();

        public virtual void PlayerReload()
        {
            ReloadFunction();
            EventManager.Instance.TriggerEvent(EventName.TURN_END);
        }

        public void ReloadFunction()
        {
            reloadCounter++;
            if (reloadCounter >= reloadTurn)
            {
                int amountToSubstract = maxMagSize - currentMagSize;
                if (ammoSize >= amountToSubstract)
                {
                    ammoSize -= amountToSubstract;
                    currentMagSize += amountToSubstract;
                }
                else
                {//less than the amount to substract than just make the ammo 0
                    currentMagSize += ammoSize;
                    ammoSize = 0;
                }
                reloadCounter = 0;
                HaveReloaded = true;
            }
        }

        public void ReloadCheckCompleted()
        {
            HaveReloaded = false;
        }

        public virtual void FireBullet()
        {
            if (currentMagSize > 0)
            {
                MethodToFireBullet();
                currentMagSize--;
            }
        }

        public void FireBulletWithoutSpending() 
        {
            MethodToFireBullet();
        }

        public void LocalRotateWeapon(float angle)
        {
            gun.localRotation= Quaternion.Euler(0,0,angle);
        }

        public abstract void ShowTrajectory();
        public abstract void HideTrajectory();

        /// <summary>
        /// Upgrade using the index of the perks as to upgrade
        /// </summary>
        /// <param name="i">perk index</param>
        public abstract void Upgrade(int i);

        public  void AddPerk (PerkBase perk)
        {
            perkGunGain.Add(perk);
        }
    }

    

}