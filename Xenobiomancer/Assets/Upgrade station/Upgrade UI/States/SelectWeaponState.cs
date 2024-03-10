using Bioweapon;
using Patterns;
using UnityEngine;

namespace UpgradeStation
{
    public class SelectWeaponState : UpgradeUIState
    {

        public SelectWeaponState(FSM fsm, UpgradeUI ui) : base(fsm, ui)
        {
            mId = (int)UpgradeState.SELECTWEAPONSTATE;
        }

        public override void Enter()
        {
            ui.SetUpWeaponButton();
            //have nothing to select
            ui.PrepareWeaponSelection();    
        }
         

        public override void Update()
        {
            
            if (ui.Player.PlayerWeapon.GunType != GunType.Pistol)
            {
                //change to the perk upgrade state
                mFsm.SetCurrentState((int)UpgradeState.SELECTPERKSTATE);
            }
        }

    }
}