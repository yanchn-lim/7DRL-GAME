using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using System.Security.Cryptography;
using Mono.Cecil;
using Bioweapon;

public enum PlayerStateType
{
    IDLE = 0,
    MOVEMENT,
    ATTACK
}

public class PlayerState : FSMState
{
    protected Player mPlayer = null;

    public PlayerState(Player player)
        : base()
    {
        mPlayer = player;
        mFsm = mPlayer.fsm;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class PlayerState_IDLE : PlayerState
{
    public PlayerState_IDLE(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.IDLE);
    }

    public override void Enter()
    {
        mPlayer.ChangeToIdleInformation();
    }

    public override void Update()
    {

        //mPlayer.MoveInputCheck();
        //if (mPlayer.MoveCheck && !GameManager.Instance.IsStillInStartState)
        //{
        //    //that means that the player has completed their action that is decided and the game manager is waiting 
        //    //the players next movement
        //    mPlayer.fsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        //}

        if (!GameManager.Instance.IsStillInStartState)
        {
            //Set the tutorial text here!
            if(PreviousState == null)
            {
                mPlayer.fsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
            }
            else
            {
                mPlayer.fsm.SetCurrentState(this.PreviousState);
            }

        }
        //if it is still running then continue to wait until it is over
    }

}

public class PlayerState_MOVEMENT : PlayerState
{
    public PlayerState_MOVEMENT(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.MOVEMENT);
    }

    public override void Enter()
    {
        base.Enter();
        mPlayer.LineRenderHandler.EnableLineRenderer();
        mPlayer.ChangeToMoveInformation();
        EventManager.Instance.AddListener(EventName.TURN_END , PlayerDecidedMovement);
    }

    public override void Update()
    {
        if(Input.GetKeyUp(KeyCode.W))
        {
            mFsm.SetCurrentState((int)PlayerStateType.ATTACK);
        }
        else
        {
            mPlayer.Moving();
        }
    }

    public override void Exit()
    {
        mPlayer.LineRenderHandler.DisableLineRenderer();
        EventManager.Instance.RemoveListener(EventName.TURN_END, PlayerDecidedMovement);
    }

    private void PlayerDecidedMovement()
    {
        mPlayer.fsm.SetCurrentState((int)PlayerStateType.IDLE);
    }
    
}

public class PlayerState_ATTACK : PlayerState
{
    public PlayerState_ATTACK(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.ATTACK);
    }

    public override void Enter()
    {
        EventManager.Instance.AddListener(EventName.TURN_END, PlayerDecidedMovement);
        mPlayer.ChangeToAttackInformation();
    }

    public override void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        }
        else
        {
            mPlayer.Weapon.UpdateFunction();
        }
    }

    public override void Exit()
    {
        EventManager.Instance.RemoveListener(EventName.TURN_END, PlayerDecidedMovement);
    }

    private void PlayerDecidedMovement()
    {
        mPlayer.fsm.SetCurrentState((int)PlayerStateType.IDLE);
    }

}



