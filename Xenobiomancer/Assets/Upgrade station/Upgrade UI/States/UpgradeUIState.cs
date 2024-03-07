using Patterns;

namespace UpgradeStation
{
    public class UpgradeUIState : FSMState
    {
        protected UpgradeUI ui;
        public UpgradeUIState(FSM fsm, UpgradeUI ui) : base(fsm)
        {
            this.ui = ui;
        }
    }
}