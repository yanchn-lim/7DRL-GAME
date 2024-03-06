using System.Collections;
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
        [Range(0, 1)]
        [SerializeField] private float endingBeamWidth;
        [Range(0,1)]
        [Tooltip("a percentage of it to charge up before shooting (count in amount of sec in a turn)")]
        [SerializeField] private float chargeUpBeam;
        [Range(0, 1)]
        [Tooltip("a percentage of it to retract back after shooting(count in amount of sec in a turn)")]
        [SerializeField] private float recoverytimeForBeam;



        public override void HideTrajectory()
        {
            trajectory.gameObject.SetActive(false);
            LineRenderHandler.DisableLineRenderer();
        }

        public override void ShowTrajectory()
        {
            trajectory.gameObject.SetActive(true);
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
    }
}