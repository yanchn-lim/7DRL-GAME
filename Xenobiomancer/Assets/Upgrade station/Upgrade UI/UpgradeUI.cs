using Bioweapon;
using Patterns;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeStation
{
    public class UpgradeUI : MonoBehaviour
    {
        private Player player;
        private FSM fsm;

        [SerializeField] private GameObject overallPanel;
        [SerializeField] private UpgradeStationData data;

        #region select weapon panel
        private Weapon weaponSelected;
        private Weapon[] currentWeapons;
        [Header("select weapon related")]
        [SerializeField] private WeaponSelectionButton weaponSelectionPrefab;
        [SerializeField] private Transform gunSelectionPanel;
        #endregion

        #region perk panel
        [Header("select perk related")]
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Transform perkSelectionPanel;
        [SerializeField] private PerkSelectionButton perkSelectionPrefab;
        private List<PerkSelectionButton> perkSelectionButtons = new List<PerkSelectionButton>();
        #endregion

        public Player Player { get => player; }
        public UpgradeStationData Data { get => data; }

        private void Start()
        {
            HidePanel();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            currentWeapons = player.AvaliableWeapons;

            fsm = new FSM();
            fsm.Add((int)UpgradeState.SELECTWEAPONSTATE, new SelectWeaponState(fsm,this));
            fsm.Add((int)UpgradeState.SELECTPERKSTATE, new PerkUpgradeState(fsm, this));

            if (Player.PlayerWeapon.GunType != GunType.Pistol)
            {
                fsm.SetCurrentState((int)UpgradeState.SELECTPERKSTATE);
            }
            else
            {
                fsm.SetCurrentState((int)UpgradeState.SELECTWEAPONSTATE);
            }
        }

        private void Update()
        {
            fsm.Update();
        }

        #region general UI function
        /// <summary>
        /// when the player click on the close button
        /// </summary>
        public void OnCloseButton()
        {
            EventManager.Instance.TriggerEvent(EventName.LeaveUpgradeStation);
            HidePanel();

        }

        private void HidePanel()
        {
            overallPanel.SetActive(false);
        }

        /// <summary>
        /// Display the upgrade UI for the player 
        /// </summary>
        public void ShowUpgradeUI()
        {
            EventManager.Instance.TriggerEvent(EventName.UseUpgradeStation);
            overallPanel.SetActive(true);
        }

        public void ClearDescription()
        {
            descriptionText.text = string.Empty;
        }
        #endregion

        #region related to weapon selection
        /// <summary>
        /// Set the submit button and values to prepare for selection of weapon
        /// </summary>
        public void PrepareWeaponSelection()
        {
            weaponSelected = null;
            gunSelectionPanel.gameObject.SetActive(true);
            perkSelectionPanel.gameObject.SetActive(false);
        }
        /// <summary>
        /// display the avaliable weapons to the player
        /// </summary>
        public void SetUpWeaponButton()
        {
            for(int i = 0; i < currentWeapons.Length; i++)
            {
                var button = Instantiate(weaponSelectionPrefab, gunSelectionPanel);
                button.GetComponent<WeaponSelectionButton>().Init(currentWeapons[i] , this);
            }
        }
        //when player click to select the weapon they want
        public void SelectWeapon(Weapon weapon)
        {
            weaponSelected = weapon;
            descriptionText.text = weapon.ShortDescription;
        }
        //when player click the submit button
        public void SubmitWeapon()
        {
            if (player.CanSpendAmount(weaponSelected.Cost))
            {
                player.SwitchWeapon(weaponSelected);
            }
            else
            {
                CantBuyPerkText();
            }
            //EventManager.Instance.TriggerEvent(EventName.LeaveUpgradeStation);
        }
        #endregion

        #region upgrade perk
        public void PreparePerkSelection()
        {
            gunSelectionPanel.gameObject.SetActive(false);
            perkSelectionPanel.gameObject.SetActive(true);
        }

        public void CreatePerkSelection(List<PerkBase> perks)
        {
            if (perks == null) Debug.LogError("error");

            //do make the check if there is nothing
            for(int i = 0; i < perks.Count;i++)
            {
                var perkButton = Instantiate(perkSelectionPrefab, perkSelectionPanel).GetComponent<PerkSelectionButton>();
                perkButton.Init(perks[i],this);
                perkSelectionButtons.Add(perkButton);
            }
        }

        public void CheckIfPerkStillExist()
        {
            List<PerkSelectionButton> perks = new List<PerkSelectionButton>(perkSelectionButtons);
            foreach(var perkButton in perks)
            {
                if (player.PlayerWeapon.PerkGunGain.Contains(perkButton.AssignPerk))
                {
                    perkSelectionButtons.Remove(perkButton);
                    perkButton.gameObject.SetActive(false);
                    ClearDescription();
                }
            }
        }

        public void ShowDescriptionOfPerk(PerkBase perk)
        {
            descriptionText.text = perk.Description;
        }

        public void CantBuyPerkText()
        {
            descriptionText.text = "Cant buy perk!!";
        }

        #endregion
    }
}