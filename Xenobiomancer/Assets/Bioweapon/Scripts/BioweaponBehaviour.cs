using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Bioweapon
{
    public class BioweaponBehaviour : MonoBehaviour
    {
        //This is only temporary for the gun and firing position
        [SerializeField] private BioweaponScriptableObject data; //the data that is relevant to the player
        [SerializeField] private Transform gun; //the gun the play is using
        [SerializeField] private Transform firingPosition; //where the bullets will be fired at

        //bullet related
        [SerializeField] private Transform poolContainer; // the pool container of bullet the player will have
        [SerializeField] private Bullet bulletPrefab; //The prefab of the bullet to use when player fire
        private PoolingPattern<Bullet> poolOfBullet; // the pool that will be used
        //update and rotate the position of the gun
        

        private void Start()
        {
            //this is to prevent any missing variable that the you might miss
            if (gun == null) Debug.LogError("No gun attact!");
            if (firingPosition == null) Debug.LogError("No firing position indicated");
            if (data == null) Debug.LogError("No Data indicated");


            SetUpBullet(); //set up the pool 
            EventManager.Instance.AddListener(EventName.TURN_COMPLETE, (Action)StopFiringBulletOnTurnComplete); //make sure the bullets wont fire after the turn is completed
        }

        //this will be called in the attack state of the player
        public void UpdateFunction()
        {
            
            RotateGunBasedOnMousePosition();
            if (Input.GetKeyUp(KeyCode.Space))
            {
                FireBullet();
            }
        }

        #region bullet related
        private void SetUpBullet()
        {
            bulletPrefab.Init(data, this);
            poolOfBullet = new PoolingPattern<Bullet> (bulletPrefab.gameObject);
            poolOfBullet.InitWithParent(10 , poolContainer , InitCommand);
        }

        private Bullet InitCommand(Bullet bulletComponent)
        {
            bulletComponent.Init(data, this);
            return bulletComponent;
        }

        #endregion

        private void RotateGunBasedOnMousePosition()
        {
            // Get the position of the mouse cursor in world coordinates
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; // Make sure z position is zero for 2D

            // Calculate direction from the object to the mouse cursor
            Vector3 direction = mousePos - transform.position;

            // Calculate the angle from the current forward direction to the direction to the cursor
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the object towards the cursor
            float eulerZAngle = Quaternion.AngleAxis(angle, Vector3.forward).eulerAngles.z;

            //quad 1: 0 -90
            //quad 2: 90-180
            //quad 3: 180-270 flip to y 180 and z
            //quad 4: 270-360
            //deciding the direction the gun should face
            if( (0f <= eulerZAngle && eulerZAngle <= 90f) || (270f <= eulerZAngle && eulerZAngle <= 360f))
            {
                gun.rotation = Quaternion.Euler(0, 0, eulerZAngle);
            }
            else
            { 
                print("hello");
                eulerZAngle = -(eulerZAngle - 180);
                gun.rotation = Quaternion.Euler(0, 180, eulerZAngle);
            }
        }

        private void FireBullet()
        {
            EventManager.Instance.TriggerEvent(EventName.TURN_END);//completed their turn
            StartCoroutine(FireBulletCoroutine());
        }

        private IEnumerator FireBulletCoroutine()
        {
            for(int i = 0; i < data.BulletFiredPerTurn; i++)
            {
                SpawnBullet();
                yield return new WaitForSeconds(data.BulletSpawnInterval);
            }
        }


        //make sure to stop the coroutine after the turn is completed
        private void StopFiringBulletOnTurnComplete()
        {
            StopAllCoroutines();
        }

        private void SpawnBullet()
        {
            var bullet = poolOfBullet.Get();
            bullet.transform.position = firingPosition.position;
            bullet.transform.rotation = gun.rotation;
            bullet.FireBullet();
        }

        public void ReturnBullet(Bullet bullet)
        {
            poolOfBullet.Retrieve(bullet);
        }

        
    }
}