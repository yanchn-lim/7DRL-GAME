using Bioweapon;
using NUnit.Framework;
using Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace UpgradeStation
{
    public class PerkUpgradeState : UpgradeUIState
    {
        private PerkBase[] perks;
        public PerkUpgradeState(FSM fsm, UpgradeUI ui) : base(fsm, ui)
        {
            mId = (int)UpgradeState.SELECTPERKSTATE;
        }

        public override void Enter()
        {
            ui.PreparePerkSelection();
            if(perks == null) //if there is no perk in the upgrade state
            {
                List<PerkBase> avaliablePerks = GetAvaliablePerks();
                var selectedPerks = GetRandomPerks(avaliablePerks);
                ui.CreatePerkSelection(selectedPerks);
                //after this we have gather all the perks that the player

            }
        }

        public override void Update()
        {
            ui.CheckIfPerkStillExist();
        }

        private List<PerkBase> GetRandomPerks(List<PerkBase> avaliablePerks)
        {
            if(avaliablePerks.Count <= 3)
            { //ignore if less or equal to 3 because that are the only option the player is going to have.
                return avaliablePerks;
            }

            List<PerkBase> perkToSelect = new List<PerkBase>();
            List<int> avaliableNumber = new List<int>();
            for(int i = 0; i < avaliablePerks.Count; i++)
            {
                avaliableNumber.Add(i);
            }//add all the avaliable number 

            for(int i = 0; i < 3; i++)
            {
                //get a random number from 0 to the end of the index
                int randomIndex = UnityEngine.Random.Range(0, avaliableNumber.Count);
                int randNum = avaliableNumber[randomIndex ];
                perkToSelect.Add(avaliablePerks[randNum]);

                avaliableNumber.RemoveAt(randomIndex); //remove that choice from the avaliable choices
            }//from here we have three different perks
            return perkToSelect;

        }

        private List<PerkBase> GetAvaliablePerks()
        {
            var allPerk = SelectPerkBasedOnWeapon();
            List<PerkBase> avaliablePerks = new List<PerkBase>();
            Weapon weaponReference = ui.Player.PlayerWeapon;

            for (int i = 0; i < allPerk.Length; i++)
            {
                if (!weaponReference.PerkGunGain.Contains(allPerk[i]))
                {//check if the weapon does not have the perk
                    avaliablePerks.Add(allPerk[i]);
                }
            }

            return avaliablePerks;
        }

        private PerkBase[] SelectPerkBasedOnWeapon()
        {
            switch(ui.Player.CurrentGunType)
            {
                case(GunType.Rifle): 
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
}