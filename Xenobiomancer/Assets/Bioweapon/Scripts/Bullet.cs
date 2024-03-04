using Patterns;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Bioweapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRender;
        private BioweaponScriptableObject data;
        private BioweaponBehaviour bioweapon;

        private bool canMoveBullet;
        //private bool firstShot; //write comment here
        private int countDownBeforeDestory;

        private void Start()
        {
            canMoveBullet = false;

        }

        private void Update()
        {
            if(canMoveBullet )
            {//add accleration later on
             //do raycasting here as well
                RaycastToHit();
                MoveBullet();
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

            TryRandomiseBullet();
            //add the event to listen if it can move or not
            EventManager.Instance.AddListener(EventName.TURN_START, (Action)TriggerMoveBullet);
            EventManager.Instance.AddListener(EventName.TURN_COMPLETE, (Action)TriggerStopBullet);
        }

        private void TryRandomiseBullet()
        {
            //var generator = new Random();
            float randValue = UnityEngine.Random.value;
            if(randValue > data.Accuracy)
            {//then do randomly displace the bullet
                float angleOfDisplacement = UnityEngine.Random.Range(-data.AngleOfOffset, data.AngleOfOffset);
                var eularAngle = new Vector3(0, 0, angleOfDisplacement);
                transform.Rotate(eularAngle);
            }

        }

        private void RevertEvent()
        {
            EventManager.Instance.RemoveListener(EventName.TURN_START, (Action)TriggerMoveBullet);
            EventManager.Instance.RemoveListener(EventName.TURN_COMPLETE, (Action)TriggerStopBullet);

        }

        #region basic function to hit the bullet
        //simple function to move the bullet
        private void MoveBullet()
        {
            transform.Translate(transform.right * 
                data.BulletSpeedPerTurn * 
                Time.deltaTime, Space.World);
        }
        private void RaycastToHit()
        {
            var hit = Physics2D.CircleCastAll(transform.position,
                data.BulletHitBoxRadius,
                Vector2.zero,
                0f);

            if(hit.Length > 0)
            {
                print("hit object");
                ReturnBullet();
            }
        }
        #endregion

        #region event base function
        private void TriggerStopBullet()
        {
            canMoveBullet = false;
            countDownBeforeDestory++;
            if(countDownBeforeDestory >= data.BulletKillTimer)
            {
                //if more than stop
                ReturnBullet();
            }
        }
        private void TriggerMoveBullet()
        {
            canMoveBullet = true;
        }
        #endregion

        private void ReturnBullet()
        {
            countDownBeforeDestory = 0;
            RevertEvent();

            bioweapon.ReturnBullet(this);
        }



        //to show the circle raycast
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, data.BulletHitBoxRadius);
        }
    }
}