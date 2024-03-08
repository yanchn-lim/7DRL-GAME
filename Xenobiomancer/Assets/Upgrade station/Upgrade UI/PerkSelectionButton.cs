using Bioweapon;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeStation;

public class PerkSelectionButton : DoubleClickButton 
{
    [SerializeField] private TextMeshProUGUI perkText;
    [SerializeField] private Image perkImage;
    private PerkBase assignPerk;
    private UpgradeUI ui;

    public PerkBase AssignPerk { get => assignPerk;  }



    public override void ClickFirstTime()
    {
        ui.ShowDescriptionOfPerk(AssignPerk);
    }

    public override void ClickSecondTime()
    {
        PerkBase[] perks = SelectPerkBasedOnWeapon();
        for (int i = 0; i < perks.Length; i++)
        {
            if (AssignPerk == perks[i])
            {

                ui.Player.PlayerWeapon.Upgrade(i);
            }
        }
    }

    public override void ExitToggleState()
    {
        ui.ClearDescription();
    }

    public void Init(PerkBase perk, UpgradeUI ui)
    {
        assignPerk = perk;
        perkText.text = assignPerk.NameOfPerk;
        perkImage.sprite = assignPerk.Logo;
        this.ui = ui;
    }


    private PerkBase[] SelectPerkBasedOnWeapon()
    {
        switch (ui.Player.CurrentGunType)
        {
            case (GunType.Rifle):
                return ui.Data.RiflePerks;
            case (GunType.ShotGun):
                return ui.Data.ShotgunPerks;
            case (GunType.Laser):
                return ui.Data.LasergunsPerk;
            case (GunType.Sniper):
                return ui.Data.SniperPerks;
            default: return null;
        }
    }

}
