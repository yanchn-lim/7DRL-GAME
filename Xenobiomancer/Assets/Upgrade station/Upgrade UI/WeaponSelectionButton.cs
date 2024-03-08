using Bioweapon;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeStation
{
    public class WeaponSelectionButton : DoubleClickButton
    {
        private Weapon assignWeapon;
        private UpgradeUI ui;
        [SerializeField] private TextMeshProUGUI nameOfWeapon;
        [SerializeField] private TextMeshProUGUI costOfWeapon;
        [SerializeField] private Button button;

        public override void ClickFirstTime()
        {
            ui.SelectWeapon(assignWeapon);
        }

        public override void ClickSecondTime()
        {    
            ui.SubmitWeapon();
        }


        public void Init(Weapon weapon , UpgradeUI usedUI)
        {
            assignWeapon = weapon;
            ui = usedUI;
            nameOfWeapon.text = weapon.NameOfTheWeapon;
            costOfWeapon.text = "cost: " + weapon.Cost;

            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            ui.SelectWeapon(assignWeapon);
        }
    }
}