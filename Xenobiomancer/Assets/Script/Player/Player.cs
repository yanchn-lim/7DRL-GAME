using Bioweapon;
using Patterns;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class Player : Stats, IDamageable
{

    [SerializeField] private Weapon weapon;
    public Weapon PlayerWeapon { get => weapon; }
    //text to help the player
    [SerializeField] private TextMeshProUGUI informationText;

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
        fsm.Add(new PlayerState_IDLE(this));
        fsm.Add(new PlayerState_MOVEMENT(this));
        fsm.Add(new PlayerState_ATTACK(this));
        fsm.Add(new PlayerState_RELOAD(this));
        fsm.SetCurrentState((int)PlayerStateType.IDLE);


        InitializeStats(playerData.health, playerData.health, playerData.currency, playerData.travelDistance);
    }

    void Update()
    {
        fsm.Update();
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
    }

    public override void DecreaseHealth(float amount)
    {
        base.DecreaseHealth(amount);
    }

    public override void IncreaseCurrency(float amount)
    {
        base.IncreaseCurrency(amount);
    }

    public override void DecreaseCurrency(float amount)
    {
        base.DecreaseCurrency(amount); 
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

}
