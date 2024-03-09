using Bioweapon;
using enemyT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using System.Runtime.CompilerServices;
using TMPro;

public class BossEnemy : EnemyBase
{
    [Header("weapons")]
    [SerializeField] private Weapon laser;
    [SerializeField] private Weapon leftRifle;
    [SerializeField] private Weapon rightRifle;
    [SerializeField] private Weapon leftShotGun;
    [SerializeField] private Weapon rightShotGun;

    [Header("Laser firing detail")]
    [Range(0, 1)]
    [SerializeField] private float laserPinPointAccuracy;
    [Range(0, 30)]
    [SerializeField] private float laserAngleOfInaccuracy;
    [Tooltip("In terms of turn time")]
    [Range(0, 1)]
    [SerializeField] private float timeToPointToPlayerLaser;

    [Header("Shotgun firing detail")]
    [Range(0, 60)]
    [SerializeField] private float shotgunAngleOfSpread;
    [Range(0,1)]
    [SerializeField] private float shotgunTimeToPoint;

    [Header("Rifle firing detail")]
    [Range(0, 1)]
    [SerializeField] private float rifleTimeToSpread;
    [Range(0, 90)]
    [SerializeField] private float rifleAngleOfSpread;

    [Header("variables")]
    [SerializeField] private float attackableRange;

    #region getter
    public Weapon Laser { get => laser; }
    public Weapon LeftRifle { get => leftRifle; }
    public Weapon RightRifle { get => rightRifle; }
    public Weapon LeftShotGun { get => leftShotGun; }
    public Weapon RightShotGun { get => rightShotGun; }
    public float LaserPinPointAccuracy { get => laserPinPointAccuracy; }
    public float LaserAngleOfInaccuracy { get => laserAngleOfInaccuracy; }
    public float TimeToPointToPlayerLaser { get => timeToPointToPlayerLaser; }
    public float AttackableRange { get => attackableRange; }
    public float ShotgunAngleOfSpread { get => shotgunAngleOfSpread; }
    #endregion
    protected override void SetupFSM()
    {
        fsm = new FSM();
        fsm.Add((int)EnemyState.IDLE, new BossIdleState(fsm,this));
        fsm.Add((int)EnemyState.CHASING, new BossChasingState(fsm, this));
        fsm.Add((int)EnemyState.ATTACKSTATE, new BossAttackingState(fsm, this));
        //fsm.Add((int)EnemyState.DEATHSTATE, new BasicDeathState(fsm, this));

        fsm.SetCurrentState((int)EnemyState.IDLE);
    }

    protected override void StartDeath()
    {
        gameObject.SetActive(false);
    }

    #region laser
    public void PrepareFireLaserCoroutine(Vector2 targetPosition)
    {
        StartCoroutine(LaserCoroutine(targetPosition));
    }

    private IEnumerator LaserCoroutine(Vector2 targetPosition)
    {
        print(targetPosition);
        float elapseTime = 0f;
        float timeToReach = TimeToPointToPlayerLaser * GameManager.Instance.TurnTime;
        Vector2 originalPosition = laser.FiringPosition.position;

        while (elapseTime < timeToReach)
        {
            elapseTime += Time.deltaTime;
            laser.RotateGunBasedOnPosition(Vector2.Lerp(originalPosition,targetPosition,elapseTime/timeToReach));
            yield return null;
        }
        laser.RotateGunBasedOnPosition(targetPosition);
        laser.FireBulletWithoutSpending();
    }
    #endregion

    #region shotgun related

    public void PrepareFireShotgunCoroutine( Vector2 centrePosition)
    {

        StartCoroutine(FireShotgunCoroutine(centrePosition));
    }

    private IEnumerator FireShotgunCoroutine( Vector2 targetPosition)
    {
        float elapseTime = 0f;
        float timeToMoveBetweenPoint = shotgunTimeToPoint * GameManager.Instance.TurnTime;

        Vector2 originalPosition = transform.position + (transform.up * (transform.position - (Vector3)targetPosition).magnitude);
        while (elapseTime < timeToMoveBetweenPoint)
        {
            elapseTime += Time.deltaTime;
            float progress = elapseTime / timeToMoveBetweenPoint;
            RotateToFacePoint(Vector2.Lerp(originalPosition,targetPosition,progress));
            yield return null;
        }

        ShotShotguns();

        

        void ShotShotguns()
        {
            leftShotGun.FireBulletWithoutSpending();
            rightShotGun.FireBulletWithoutSpending();
        }
    }
    #endregion

    #region rifle related

    public void PrepareFireRifleCoroutine(Vector2 targetPosition)
    {

        StartCoroutine(FireRifleCoroutine(targetPosition));
    }

    private IEnumerator FireRifleCoroutine(Vector2 targetPosition)
    {
        float elapseTime = 0f;
        float timeToMoveBetweenPoint = rifleTimeToSpread * GameManager.Instance.TurnTime;

        //Vector2 originalPosition = transform.position + (transform.up * (transform.position - (Vector3)targetPosition).magnitude);
        //while (elapseTime < timeToMoveBetweenPoint)
        //{
        //    elapseTime += Time.deltaTime;
        //    float progress = elapseTime / timeToMoveBetweenPoint;
        //    RotateToFacePoint(Vector2.Lerp(originalPosition, targetPosition, progress));
        //    yield return null;
        //}

        elapseTime = 0f;
        leftRifle.FireBulletWithoutSpending();
        rightRifle.FireBulletWithoutSpending();
        while (elapseTime < timeToMoveBetweenPoint)
        {
            elapseTime += Time.deltaTime;
            float progress = elapseTime / timeToMoveBetweenPoint;
            RotateBothRifle(Mathf.Lerp(0, rifleAngleOfSpread, progress));
            yield return null;
        }
        StartCoroutine(ReturnRifleCoroutine());
        
    }

    private IEnumerator ReturnRifleCoroutine()
    {
        float elapseTime = 0f;
        float timeToMoveBetweenPoint = GameManager.Instance.TurnTime;

        while (elapseTime < timeToMoveBetweenPoint)
        {
            elapseTime += Time.deltaTime;
            float progress = elapseTime / timeToMoveBetweenPoint;
            RotateBothRifle(Mathf.Lerp(rifleAngleOfSpread, 0 , progress));
            yield return null;
        }
    }

    void RotateBothRifle(float angle)
        {
            leftRifle.LocalRotateWeapon(angle + 90f);
            rightRifle.LocalRotateWeapon(-angle + 90f);
        }

    #endregion

    protected void RotateToFacePoint(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;

        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            var targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = targetRotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackableRange);
    }
}
