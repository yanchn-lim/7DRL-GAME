using Patterns;
using System;
using System.Collections;
using UnityEngine;

namespace Bioweapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRender;
        private BioweaponScriptableObject data;
        private BioweaponBehaviour bioweapon;

        private bool canMoveBullet;
        private bool firstShot; //write comment here
        private int countDownBeforeDestory;

        private void Start()
        {
            canMoveBullet = false;

        }

        private void Update()
        {
            if(canMoveBullet || firstShot)
            {//add accleration later on
                //do raycasting here as well

                MoveBullet();
            }
            else
            {
                //slow down the bullet
            }
        }

        public void Init(BioweaponScriptableObject data, BioweaponBehaviour bioweapon)
        {
            spriteRender.sprite = data.BulletSprite;
            this.data = data;
            this.bioweapon = bioweapon;
        }
        
        public void FireBullet()
        {
            EventManager.Instance.AddListener(EventName.TURN_START, (Action)TriggerMoveBullet);
            EventManager.Instance.AddListener(EventName.TURN_COMPLETE, (Action)TriggerStopBullet);
            firstShot = true;
        }

        private void RevertEvent()
        {
            EventManager.Instance.RemoveListener(EventName.TURN_START, (Action)TriggerMoveBullet);
            EventManager.Instance.RemoveListener(EventName.TURN_COMPLETE, (Action)TriggerStopBullet);

        }

        private void MoveBullet()
        {
            transform.Translate(transform.right * 
                data.BulletSpeedPerTurn * 
                Time.deltaTime, Space.World);
        }

        private void TriggerStopBullet()
        {
            firstShot = false;
            canMoveBullet = false;
            countDownBeforeDestory++;
            if(countDownBeforeDestory >= data.BulletKillTimer) 
            {
                //if more than stop
                countDownBeforeDestory = 0;
                RevertEvent();

                bioweapon.ReturnBullet(this);
            }
        }

        private void TriggerMoveBullet()
        {
            print("move bullet");
            canMoveBullet = true;
        }
    }
}