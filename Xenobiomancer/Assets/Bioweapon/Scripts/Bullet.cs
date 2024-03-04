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
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private ParticleSystem particleForHit;
        private BioweaponScriptableObject data;
        private BioweaponBehaviour bioweapon;

        //private bool firstShot; //write comment here
        private int countDownBeforeDestory;

        private void Update()
        {
            RaycastToHit();
            MoveBullet();
        }

        public void Init(BioweaponScriptableObject data, BioweaponBehaviour bioweapon)
        {
            spriteRender.sprite = data.BulletSprite;
            this.data = data;
            this.bioweapon = bioweapon;
        }
        
        public void FireBullet()
        {
            trailRenderer.enabled = true;
            spriteRender.enabled = true;
            TryRandomiseBullet();

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
                //print("hit object");
                //ReturnBullet();
                StartCoroutine(CollisionCoroutine());
            }
        }

        private IEnumerator CollisionCoroutine()
        {
            particleForHit.Play();
            spriteRender.enabled = false;
            while(true)
            {
                if(particleForHit.isPlaying)
                {
                    yield return null;
                }
                else
                {
                    break;
                }
            }
            ReturnBullet();
        }

        #endregion

        private void ReturnBullet()
        {
            trailRenderer.enabled = false;
            countDownBeforeDestory = 0;
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