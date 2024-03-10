using System.Collections;
using TMPro;
using UnityEngine;

namespace Bioweapon
{
    public class Lasergun : Weapon
    {
        [Header("Laser details")]        
        [SerializeField] private LineRenderHandler LineRenderHandler;
        [SerializeField] private GameObject trajectory;
        [SerializeField] private float lengthOfBeam;

        [Range(0,1)]
        [SerializeField] private float startingBeamWidth;
        [Range(0, 5)]
        [SerializeField] private float endingBeamWidth;
        [Range(0,1)]
        [Tooltip("a percentage of it to charge up before shooting (count in amount of sec in a turn)")]
        [SerializeField] private float chargeUpBeam;
        [Range(0, 1)]
        [Tooltip("a percentage of it to retract back after shooting(count in amount of sec in a turn)")]
        [SerializeField] private float recoverytimeForBeam;
        [SerializeField] private int damage;

        public override void HideTrajectory()
        {
            trajectory.gameObject.SetActive(false);
            LineRenderHandler.DisableLineRenderer();
        }

        public override void ShowTrajectory()
        {
            trajectory.gameObject.SetActive(true);
        }

        public void UpgradeLaser(LasergunPerk perk)
        {
            endingBeamWidth += perk.IncreaseBeamWidth;
            lengthOfBeam += perk.IncreaseBeamLength;
            chargeUpBeam -= perk.ReduceChargeUp;
            damage += perk.Damage;
            maxMagSize += perk.IncreaseMaxAmmo;
        }

        public override void Upgrade(int i)
        {
            LasergunPerk perk = upgradeData.LasergunsPerk[i];
            UpgradeLaser(perk);
            perkGunGain.Add(perk);
        }

        protected override void MethodToFireBullet()
        {
            StartCoroutine(ShootBeam());
        }

        private IEnumerator ShootBeam()
        {
            Vector2 finalDirection = (firingPosition.position - gun.position).normalized * lengthOfBeam + firingPosition.position;
            LineRenderHandler.EnableLineRenderer();
            LineRenderHandler.MovementLine(firingPosition.position, finalDirection);

            float chargeUpTime = GameManager.Instance.TurnTime * chargeUpBeam;
            float elapseTime = 0f;
            while(elapseTime < chargeUpTime)
            {
                LineRenderHandler.SetWidth(Mathf.Lerp(startingBeamWidth, endingBeamWidth, elapseTime / chargeUpTime));
                elapseTime += Time.deltaTime;
                yield return null;
            }
            LineRenderHandler.SetWidth(endingBeamWidth);

            Vector2 rectangeSize = new Vector2(endingBeamWidth, lengthOfBeam);

            Vector2 middlePoint = ((Vector2)firingPosition.position + finalDirection) / 2f;

            float angle = gun.transform.rotation.eulerAngles.z;
            if(gun.transform.eulerAngles.y <= 0)
            {
                angle += 90f;
            }
            else
            {
                angle = -angle + 90f; 

            }

            var hits = Physics2D.BoxCastAll(middlePoint,
                rectangeSize,
                angle,
                Vector2.zero
            );




            foreach( var hit in hits )
            {
                if(hit.collider != null)
                {
                    if(hit.collider.TryGetComponent<IDamageable>(out var component))
                    {
                        component.TakeDamage(damage);
                    }
                }
            }


            elapseTime = 0f;
            float recoverTime = GameManager.Instance.TurnTime * recoverytimeForBeam;
            while (elapseTime < chargeUpTime)
            {
                LineRenderHandler.SetWidth(Mathf.Lerp(endingBeamWidth,startingBeamWidth , elapseTime / recoverTime));
                elapseTime += Time.deltaTime;
                yield return null;
            }

            LineRenderHandler.DisableLineRenderer();
        }
        void DrawDebugRectangle(Vector2 center, float width, float height, float angleDegrees, Color color)
        {
            float angleRadians = angleDegrees * Mathf.Deg2Rad;
            Vector2[] corners = new Vector2[4];

            // Calculate half extents of the rectangle
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;

            // Calculate the rotated corners of the rectangle
            corners[0] = RotatePoint(new Vector2(-halfWidth, -halfHeight), angleRadians) + center;
            corners[1] = RotatePoint(new Vector2(halfWidth, -halfHeight), angleRadians) + center;
            corners[2] = RotatePoint(new Vector2(halfWidth, halfHeight), angleRadians) + center;
            corners[3] = RotatePoint(new Vector2(-halfWidth, halfHeight), angleRadians) + center;

            // Draw lines between the corners to form the rectangle
            Debug.DrawLine(corners[0], corners[1], color, 1f);
            Debug.DrawLine(corners[1], corners[2], color, 1f);
            Debug.DrawLine(corners[2], corners[3], color, 1f);
            Debug.DrawLine(corners[3], corners[0], color, 1f);
        }

        Vector2 RotatePoint(Vector2 point, float angle)
        {
            float x = point.x * Mathf.Cos(angle) - point.y * Mathf.Sin(angle);
            float y = point.x * Mathf.Sin(angle) + point.y * Mathf.Cos(angle);
            return new Vector2(x, y);
        }


    }
}