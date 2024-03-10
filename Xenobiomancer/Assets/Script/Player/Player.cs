using Bioweapon;
using Patterns;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Stats, IDamageable
{
    [SerializeField] private bool testing;
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
    [SerializeField]private int healAmount = 20;
    [SerializeField]private int costToHeal = 100;
    

    [HideInInspector]
    public FSM fsm = new FSM();
    public PlayerMovement PlayerMovement;
    public LineRenderHandler LineRenderHandler;
    public Camera Main;
    [SerializeField]
    private PlayerData playerData;
    [SerializeField] private float invincibilityDuration;
    private bool isInvincible;

    public bool MoveCheck;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField] private float radiusToStartMap;

    [SerializeField]
    Tilemap fogMap,obstacleMap,tileMap;
    [SerializeField]
    int visionRange;

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
        fsm.Add(new PlayerState_DEATH(this));
        if (testing)
        {
            fsm.SetCurrentState((int)PlayerStateType.IDLE);

        }
        else
        {
            fsm.SetCurrentState((int)PlayerStateType.MAP);
        }

        InitializeStats(playerData.health, playerData.health, playerData.currency, playerData.travelDistance);
        displayStats.SetUI(Health, MaxHealth, Currency);
        displayStats.ChangeCostText(currentWeapon.AmmoCost);
    }

    void Update()
    {
        displayStats.SetWeaponUI(currentWeapon.CurrentMagSize, currentWeapon.MaxMagSize, currentWeapon.AmmoSize);
        fsm.Update();

        UpdateFogMap();


        float distanceFromArea = Vector2.Distance(transform.position, Vector2.zero);
        if(distanceFromArea <= radiusToStartMap)
        {
            displayStats.ShowTransferShipText();
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //OPEN MAP
                EventManager.Instance.TriggerEvent(EventName.LEVEL_COMPLETED);
            }
        }
        else
        {
            displayStats.HideTransferShipText();
        }
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

    void UpdateFogMap()
    {
        if (fogMap == null || obstacleMap == null || tileMap == null)
            return;

        Vector3Int posInt = Vector3Int.RoundToInt(transform.position);

        for (int x = -visionRange; x < visionRange + 1; x++)
        {
            for (int y = -visionRange; y <= visionRange + 1; y++)
            {
                Vector3Int surroundingPos = new(posInt.x + x, posInt.y + y,0);
                TileBase tile = fogMap.GetTile(surroundingPos);

                if(tile != null)
                {
                    fogMap.SetTile(surroundingPos,null);
                }
            }
        }
    }


    #endregion

    private void ChangeGunImage()
    {
        displayStats.ChangeWeaponImageBasedOnWeaponType(currentGunType);
    }

    public override void IncreaseHealth(int amount)
    {
        base.IncreaseHealth(amount);
        displayStats.UpdateHealthUI(Health, MaxHealth);
    }

    public override void DecreaseHealth(int amount)
    {
        base.DecreaseHealth(amount);
        displayStats.UpdateHealthUI(Health, MaxHealth);
        CheckPlayerHealth();
    }

    public override void IncreaseCurrency(int amount)
    {
        base.IncreaseCurrency(amount);
        displayStats.UpdateCurrencyUI(Currency);
    }

    public override void DecreaseCurrency(int amount)
    {
        base.DecreaseCurrency(amount);
        displayStats.UpdateCurrencyUI(Currency);
    }

    public override void IncreaseTravelDistance(float amount)
    {
        base.IncreaseTravelDistance(amount);
    }

    public override void DecreaseTravelDistance(float amount)
    {
        base.DecreaseTravelDistance(amount);
    }

    public override void InitializeStats(int initialHealth, int maxHealth, int initialCurrency, float intialTravelDistance)
    {
        base.InitializeStats(initialHealth, maxHealth, initialCurrency, intialTravelDistance);
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            StartInvincibilityFrames();
            DecreaseHealth(damage);
            CheckPlayerHealth();

        }
        
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

    public void ChangeToCantBuyAmmoInformation()
    {
        informationText.text = $"Cant buy ammo \n" +
            $"Press W to swap to Move";
    }

    public void ChangeToCantReload()
    {
        informationText.text = $"No more ammo! Ammo cost: {currentWeapon.AmmoCost} \n" +
            $"Press Q to buy Ammo \n" +
            $"Press W to swap to Move";
    }

    public void ChangeToCannotHeal()
    {
        informationText.text = $"Cant heal! Heal cost: {healAmount} \n" +
           $"Press Q to buy Ammo \n" ;
    }
    
    public void SwitchWeapon(Weapon weapon)
    {
        currentWeapon.gameObject.SetActive(false);
        currentWeapon = weapon;
        currentGunType = weapon.GunType;
        weapon.gameObject.SetActive(true);
        ChangeGunImage();
        displayStats.ChangeCostText(currentWeapon.AmmoCost);
    }


    public void HealPlayer()
    {
        if (Currency >= costToHeal)
        {
            IncreaseHealth(healAmount);
            DecreaseCurrency(costToHeal);
            
            EventManager.Instance.TriggerEvent(EventName.TURN_END);
        }
        else
        {
            ChangeToCannotHeal();
        }
    }

    public void BuyAmmo()
    {
        
    }

   
    void CheckPlayerHealth()
    {
        if (Health <= 0)
        {
            EventManager.Instance.TriggerEvent(EventName.PLAYER_DEATH);
        }
    }

    public void BuyAmmoWithCurrency()
    {
        if (Currency >= PlayerWeapon.AmmoCost)
        {
            DecreaseCurrency(PlayerWeapon.AmmoCost);
            PlayerWeapon.BuyAmmo();
        }
        else 
        {
            ChangeToCantBuyAmmoInformation();
        }
    }

    public void UpgradeWithCurrency(int i, int costOfUpgrade)
    {
        if (Currency >= costOfUpgrade)
        {
            PlayerWeapon.Upgrade(i);
            DecreaseCurrency(costOfUpgrade);
        }
        else
        {
            //cannot upgrade
        }
        
    }

    private void StartInvincibilityFrames()
    {
        isInvincible = true;
        Invoke("EndInvincibilityFrames", invincibilityDuration);
    }

    private void EndInvincibilityFrames()
    {
        isInvincible = false;
    }
    

}
