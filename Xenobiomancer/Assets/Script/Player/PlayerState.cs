using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using System.Security.Cryptography;
using Mono.Cecil;

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
        base.Enter();
        Debug.Log("IDLING MIMIMIMIMI");
    }

    public override void Update()
    {
        base.Update();

        mPlayer.MoveInputCheck();
        if (mPlayer.MoveCheck)
        {
            mPlayer.fsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        }


    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
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
        Debug.Log("ITS MOVING TIME");
        mPlayer.LineRenderHandler.EnableLineRenderer();
    }

    public override void Update()
    {
        base.Update();

        mPlayer.Moving();

        if (!mPlayer.MoveCheck)
        {
            mPlayer.fsm.SetCurrentState((int)PlayerStateType.IDLE);
        }
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
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
        base.Enter();
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



