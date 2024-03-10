using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using System.Security.Cryptography;
using Mono.Cecil;
using Bioweapon;
using System;

public enum PlayerStateType
{
    IDLE = 0,
    MOVEMENT,
    ATTACK,
    RELOAD,
    INTERACT,
    MAP,
    DEATH
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
        EventManager.Instance.AddListener(EventName.TURN_END, PlayerDecidedMovement);
        EventManager.Instance.AddListener(EventName.UseUpgradeStation, PlayerInteractWithUpgradeStation);
        EventManager.Instance.AddListener(EventName.LEVEL_COMPLETED, PlayerLookMap);
        EventManager.Instance.AddListener(EventName.PLAYER_DEATH, PlayerDeath);
    }

    public override void Exit()
    {
        EventManager.Instance.RemoveListener(EventName.TURN_END, PlayerDecidedMovement);
        EventManager.Instance.RemoveListener(EventName.UseUpgradeStation, PlayerInteractWithUpgradeStation);
        EventManager.Instance.RemoveListener(EventName.LEVEL_COMPLETED, PlayerLookMap);
        EventManager.Instance.RemoveListener(EventName.PLAYER_DEATH, PlayerDeath);
    }

    private void PlayerDecidedMovement()
    {
        mPlayer.fsm.SetCurrentState((int)PlayerStateType.IDLE);
       
    }

    private void PlayerInteractWithUpgradeStation()
    {
        mFsm.SetCurrentState((int)PlayerStateType.INTERACT);
    }

    private void PlayerLookMap()
    {
        mFsm.SetCurrentState((int)PlayerStateType.MAP);
    }

    private void PlayerDeath()
    {
        mFsm.SetCurrentState((int)PlayerStateType.DEATH);
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
        
    }

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            mFsm.SetCurrentState((int)PlayerStateType.ATTACK);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            mPlayer.HealPlayer();
        }
        else
        {
            mPlayer.Moving();
        }

       
    }

    public override void Exit()
    {
        mPlayer.LineRenderHandler.DisableLineRenderer();
        base.Exit();

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
        mPlayer.ChangeToAttackInformation();
        if (mPlayer.PlayerWeapon.CanShoot)
        {
            mPlayer.PlayerWeapon.ShowTrajectory();

        }
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        }
        else if (Input.GetKeyUp(KeyCode.R) )
        {//if player wants to reload
            mFsm.SetCurrentState((int)PlayerStateType.RELOAD);
        }
        else if (!mPlayer.PlayerWeapon.CanShoot)
        {//if cant shoot
            mFsm.SetCurrentState((int)PlayerStateType.RELOAD);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            mPlayer.HealPlayer();
        }
        else
        {
            //do the weapon update loop
            mPlayer.PlayerWeapon.UpdateFunction();
        }
    }

    public override void Exit()
    {
        mPlayer.PlayerWeapon.HideTrajectory();
        base.Exit();
    }


}

public class PlayerState_RELOAD : PlayerState
{
    public PlayerState_RELOAD(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.RELOAD);
    }

    public override void Enter()
    {
        base.Enter();
        if (mPlayer.PlayerWeapon.HaveReloaded)
        {
            mPlayer.PlayerWeapon.ReloadCheckCompleted();
            mFsm.SetCurrentState((int)PlayerStateType.ATTACK);

        }
        else
        {
            if (mPlayer.PlayerWeapon.CanReload)
            {//show that it can reload
                mPlayer.ChangeToReloadingInformation();
            }
            else
            {
                mPlayer.ChangeToCantReload();
            }
        }
    }

    public override void Update()
    {
        if (mPlayer.PlayerWeapon.CanReload && Input.GetKeyDown(KeyCode.R))
        {//show that it can reload
            //mPlayer.ReloadWithCurrency();
            mPlayer.PlayerWeapon.PlayerReload();
        }
        if(!mPlayer.PlayerWeapon.CanReload && 
            Input.GetKeyDown(KeyCode.Q) )
        {
            mPlayer.BuyAmmoWithCurrency();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        }
    }




}

public class PlayerState_INTERACTing : PlayerState
{
    public PlayerState_INTERACTing(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.INTERACT);
    }

    public override void Enter()
    {
        EventManager.Instance.AddListener(EventName.LeaveUpgradeStation, (Action)SwitchToMovementState);
    }

    public override void Exit()
    {
        EventManager.Instance.RemoveListener(EventName.LeaveUpgradeStation, (Action)SwitchToMovementState);

    }

    private void SwitchToMovementState()
    {
        mFsm.SetCurrentState((int)(PlayerStateType.MOVEMENT));
    }
}

public class PlayerState_MAP : PlayerState
{
    public PlayerState_MAP(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.MAP);
    }

    public override void Enter()
    {
        mPlayer.transform.position = new(0,0,0);
        EventManager.Instance.AddListener(EventName.MAP_NODE_CLICKED, (Action)SwitchToMovementState);
    }

    public override void Exit()
    {
        EventManager.Instance.RemoveListener(EventName.MAP_NODE_CLICKED, (Action)SwitchToMovementState);

    }

    private void SwitchToMovementState()
    {
        mFsm.SetCurrentState((int)(PlayerStateType.MOVEMENT));
    }
}

public class PlayerState_DEATH : PlayerState
{
    public PlayerState_DEATH(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.DEATH);
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        

    }

    
}