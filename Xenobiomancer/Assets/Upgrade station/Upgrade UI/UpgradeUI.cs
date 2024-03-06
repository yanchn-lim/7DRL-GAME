using Bioweapon;
using Patterns;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeStation
{
    public class UpgradeUI : MonoBehaviour
    {
        private Player player;
        private FSM fsm;

        [SerializeField] private GameObject overallPanel;

        #region select weapon panel
        [Header("select weapon related")]
        private Weapon weaponSelected;
        [SerializeField] private Weapon[] currentWeapons;
        [SerializeField] private WeaponSelectionButton buttonPrefab;
        [SerializeField] private Transform gunSelectionPanel;
        #endregion

        [SerializeField] private Button submitButton;


        private void Awake()
        {
            EventManager.Instance.AddListener(EventName.UseUpgradeStation, ShowUpgradeUI);
            EventManager.Instance.AddListener(EventName.LeaveUpgradeStation, HideUpgradeUI);
        }

        private void Start()
        {
            HideUpgradeUI();

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            fsm = new FSM();
            fsm.Add((int)UpgradeState.SELECTWEAPONSTATE, new SelectWeaponState(fsm,this));
            fsm.SetCurrentState((int)UpgradeState.SELECTWEAPONSTATE);
        }

        public void OnCloseButton()
        {
            EventManager.Instance.TriggerEvent(EventName.LeaveUpgradeStation);
        }

        private void HideUpgradeUI()
        {
            overallPanel.SetActive(false);
        }

        public void ShowUpgradeUI()
        {
            overallPanel.SetActive(true);
        }

        private void Update()
        {
            fsm.Update();
        }

        public void PrepareWeaponSelection()
        {
            weaponSelected = null;
            submitButton.onClick.RemoveAllListeners();
            submitButton.onClick.AddListener(SubmitWeapon);
            submitButton.gameObject.SetActive(false);

        }

        public void SetUpWeapon()
        {
            for(int i = 0; i < currentWeapons.Length; i++)
            {
                var button = Instantiate(buttonPrefab, gunSelectionPanel);
                button.GetComponent<WeaponSelectionButton>().Init(currentWeapons[i] , this);
            }
        }

        //when player click to select the weapon they want
        public void SelectWeapon(Weapon weapon)
        {
            submitButton.gameObject.SetActive(true);
            weaponSelected = weapon;
        }
        //when player click the submit button
        private void SubmitWeapon()
        {
            player.SwitchWeapon(weaponSelected);
            EventManager.Instance.TriggerEvent(EventName.LeaveUpgradeStation);
        }


    }

    public class UpgradeUIState : FSMState
    {
        protected UpgradeUI ui;
        public UpgradeUIState(FSM fsm, UpgradeUI ui) : base(fsm)
        {
            this.ui = ui;
        }
    }

    public class SelectWeaponState : UpgradeUIState
    {
        public SelectWeaponState(FSM fsm, UpgradeUI ui) : base(fsm, ui)
        {
            mId = (int)UpgradeState.SELECTWEAPONSTATE;
        }

        public override void Enter()
        {
            ui.SetUpWeapon();
            //have nothing to select
            ui.PrepareWeaponSelection();    
        }

        
        
    }

    public enum UpgradeState
    {
        SELECTWEAPONSTATE,
        SELECTPERKSTATE
    }
}