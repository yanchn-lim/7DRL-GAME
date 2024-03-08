using Bioweapon;
using Patterns;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class Player : Stats, IDamageable
{
    [Header("Gun")]
    [SerializeField] private GunType currentGunType;
    [SerializeField] private Weapon[] avaliableWeapons;

    [Header("current weapon")]
    [SerializeField] private Weapon currentWeapon;
    public Weapon PlayerWeapon { get => currentWeapon; }
    public Weapon[] AvaliableWeapons { get => avaliableWeapons; }
    public GunType CurrentGunType { get => currentGunType;}

    [Header("HUD")]
    //text to help the player
    [SerializeField] private TextMeshProUGUI informationText;

    //Display the stats of the player
    [SerializeField] private DisplayStats displayStats;

    [HideInInspector]
    public FSM fsm = new FSM();
    public PlayerMovement PlayerMovement;
    public LineRenderHandler LineRenderHandler;
    public Camera Main;
    [SerializeField]
    private PlayerData playerData;


    public bool MoveCheck;

    [SerializeField]
    private LayerMask wallLayer;

    void Start()
    {
        //change this as the starting gun is a pistol
        currentGunType = GunType.Pistol;


        fsm.Add(new PlayerState_IDLE(this));
        fsm.Add(new PlayerState_MOVEMENT(this));
        fsm.Add(new PlayerState_ATTACK(this));
        fsm.Add(new PlayerState_RELOAD(this));
        fsm.Add(new PlayerState_INTERACTing(this));
        fsm.Add(new PlayerState_MAP(this));
        fsm.SetCurrentState((int)PlayerStateType.MAP);


        InitializeStats(playerData.health, playerData.health, playerData.currency, playerData.travelDistance);
        displayStats.SetUI(Health, MaxHealth, Currency);
    }

    void Update()
    {
        fsm.Update();

        TestSwitchScene();
    }

    #region Movement

    public void MoveInputCheck()
    {
        PlayerMovement.MovementCheck();
        MoveCheck = PlayerMovement.MoveCheck;
    }

    public void Moving()
    {
        //get the position that the player will move towards
        Vector2 targetPos = CalculateClampedPosition();

        LineRenderHandler.MovementLine(targetPos);

        //if left click then move the player
        if (Input.GetMouseButtonDown(0))
        {
            PlayerMovement.Move(targetPos);
            ResetMoveCheck();
            LineRenderHandler.DisableLineRenderer();

            //End the player turn
            EventManager.Instance.TriggerEvent(EventName.TURN_END);
        }

    }

    private Vector2 CalculateClampedPosition()
    {
        //get mouse position on the screen
        Vector2 mouseWorldPos = Main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;

        // Calculate the direction from player to mouse
        Vector2 direction = mouseWorldPos - playerPos;

        // Clamp the distance to the ClampRadius
        float clampedDistance = Mathf.Clamp(direction.magnitude, 0, TravelDistance);

        // Set the clamped position within the radius
        Vector2 clampedDirection = direction.normalized * clampedDistance;
        Vector2 targetPos = playerPos + clampedDirection;


        //check if there is a wall in the path
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, clampedDistance, wallLayer);

        if (hit.collider != null)
        {
            targetPos = hit.point;
        }

        return targetPos;
    }

    public void ResetMoveCheck()
    {
        MoveCheck = false;
    }




    #endregion

    public override void IncreaseHealth(float amount)
    {
        base.IncreaseHealth(amount);
        displayStats.UpdateHealthUI(Health, MaxHealth);
    }

    public override void DecreaseHealth(float amount)
    {
        base.DecreaseHealth(amount);
        displayStats.UpdateHealthUI(Health, MaxHealth);
    }

    public override void IncreaseCurrency(float amount)
    {
        base.IncreaseCurrency(amount);
        displayStats.UpdateCurrencyUI(amount);
    }

    public override void DecreaseCurrency(float amount)
    {
        base.DecreaseCurrency(amount);
        displayStats.UpdateCurrencyUI(amount);
    }

    public override void IncreaseTravelDistance(float amount)
    {
        base.IncreaseTravelDistance(amount);
    }

    public override void DecreaseTravelDistance(float amount)
    {
        base.DecreaseTravelDistance(amount);
    }

    public override void InitializeStats(float initialHealth, float maxHealth, float initialCurrency, float intialTravelDistance)
    {
        base.InitializeStats(initialHealth, maxHealth, initialCurrency, intialTravelDistance);
    }

    public void Damage(float damage)
    {
        DecreaseHealth(damage);
        Debug.Log(Health);
    }

    public void ChangeToAttackInformation()
    {
        informationText.text = "Press W to swap to Move \n" +
            "Press spacebar to shoot";
    }

    public void ChangeToIdleInformation()
    {
        informationText.text = "Waiting...";
    }

    public void ChangeToMoveInformation()
    {
        informationText.text = "Press W to swap to attack";
    }

    public void ChangeToReloadingInformation()
    {
        informationText.text = $"Press R to reload. You have reloaded " +
            $"{PlayerWeapon.ReloadCounter}/{PlayerWeapon.ReloadTurn} \n" +
            $"Press W to swap to Move";
    }

    public void ChangeToCantReload()
    {
        informationText.text = $"No more ammo. \n" +
            $"Press W to swap to Move";
    }
    
    public void SwitchWeapon(Weapon weapon)
    {
        currentWeapon.gameObject.SetActive(false);
        currentWeapon = weapon;
        currentGunType = weapon.GunType;
        weapon.gameObject.SetActive(true);
    }


    public void TestSwitchScene()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            EventManager.Instance.TriggerEvent(EventName.LEVEL_COMPLETED);
        }
    }

}
