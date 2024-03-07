using Patterns;

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
        

    }
}