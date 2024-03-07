using Bioweapon;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UpgradeStation;

public class PerkSelectionButton : DoubleClickButton
{
    [SerializeField] private TextMeshProUGUI perkText;
    [SerializeField] private Image perkImage;
    private PerkBase assignPerk;
    private UpgradeUI ui;

    public PerkBase AssignPerk { get => assignPerk;  }

    public override void clickFirstTime()
    {
        ui.ShowDescriptionOfPerk(AssignPerk);
    }

    public override void clickSecondTime()
    {
        //search for the perk in the array
        switch (ui.Player.CurrentGunType)
        {
            case (GunType.Rifle):
                RifleAction();
                break;
            default:
                break;
        }


    }

    public void Init(PerkBase perk, UpgradeUI ui)
    {
        assignPerk = perk;
        perkText.text = assignPerk.NameOfPerk;
        perkImage.sprite = assignPerk.Logo;
        this.ui = ui;
    }

    private void RifleAction()
    {
        RiflePerk[] perks = ui.Data.RiflePerks;
        for(int i = 0; i < perks.Length; i++)
        {
            if(AssignPerk == perks[i])
            {
                ui.Player.PlayerWeapon.Upgrade(i);
            }
        }
    }



}


public abstract class DoubleClickButton : MonoBehaviour, IPointerUpHandler
{
    private int timeClick = 0;

    public abstract void clickFirstTime();
    public abstract void clickSecondTime();

    //when the player click finish
    public void OnPointerUp(PointerEventData eventData)
    {
        if(timeClick == 0)
        {
            clickFirstTime();
            timeClick++;
        }
        else if(timeClick == 1)
        {
            clickSecondTime();
            timeClick = 0;//reset back
        }
    }
}
