using Patterns;
using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;


public class Player : Stats, IDamageable
{

    [HideInInspector]
    public FSM fsm = new FSM();
    public PlayerMovement PlayerMovement;
    public LineRenderHandler LineRenderHandler;
    public Camera Main;
    

    public bool MoveCheck;

    public float ClampRadius;
   
    


    // Start is called before the first frame update
    void Start()
    {
        fsm.Add(new PlayerState_IDLE(this));
        fsm.Add(new PlayerState_MOVEMENT(this));
        fsm.Add(new PlayerState_ATTACK(this));
        fsm.SetCurrentState((int)PlayerStateType.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }

    


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
        float clampedDistance = Mathf.Clamp(direction.magnitude, 0, ClampRadius);

        // Set the clamped position within the radius
        Vector2 clampedDirection = direction.normalized * clampedDistance;
        Vector2 targetPos = playerPos + clampedDirection;

        return targetPos;
    }

    public void ResetMoveCheck()
    {
        MoveCheck = false;
    }


    public override void DecreaseValue()
    {
        
    }

    public override void IncreaseValue()
    {
        
    }

    public void Damage(float damage)
    {
        Health -= damage;
    }



}
