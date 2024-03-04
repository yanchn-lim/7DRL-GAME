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
        [SerializeField] private BioweaponScriptableObject data;
        [SerializeField] private Transform gun;
        [SerializeField] private Transform firingPosition;

        //bullet related
        [SerializeField] private Transform poolContainer;
        [SerializeField] private Bullet bulletPrefab;
        private PoolingPattern<Bullet> poolOfBullet;
        //update and rotate the position of the gun
        

        private void Start()
        {
            if (gun == null) Debug.LogError("No gun attact!");
            if (firingPosition == null) Debug.LogError("No firing position indicated");
            if (data == null) Debug.LogError("No Data indicated");
            SetUpBullet();
            EventManager.Instance.AddListener(EventName.TURN_COMPLETE, (Action)StopFiringBulletOnTurnComplete);
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
            gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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