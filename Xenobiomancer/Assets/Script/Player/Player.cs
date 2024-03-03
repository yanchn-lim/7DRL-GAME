using Patterns;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{

    [HideInInspector]
    public FSM fsm = new FSM();
    public PlayerMovement PlayerMovement;
    

    public bool MoveCheck;
    


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

    public void MovePlayer()
    {
        PlayerMovement.Move();
    }


    public void MoveInputCheck()
    {
        PlayerMovement.MovementCheck();
        MoveCheck = PlayerMovement.MoveCheck;

        
    }

    public void ResetMoveCheck()
    {
        MoveCheck = false;
    }


}
