using Bioweapon;
using UnityEngine;

namespace UpgradeStation
{
    [CreateAssetMenu(fileName = "upgradestationData", menuName = "ScriptableObjects/Upgrade Station")]
    public class UpgradeStationData : ScriptableObject
    {
        [Header("Avaliable Perks that exist")]
        [Tooltip("perks use for shotgun")]
        [SerializeField] private ShotgunPerk[] shotgunPerks;
        [Tooltip("perks use for rifle")]
        [SerializeField] private RiflePerk[] riflePerks;
        [Tooltip("perks use for sniper")]
        [SerializeField] private SniperPerk[] sniperPerks;
        [Tooltip("perks use for lasergun")]
        [SerializeField] private LasergunPerk[] lasergunsPerk;

        [Tooltip("radius the player should be in to toggle the upgrade station")]
        [SerializeField] private float toggleRadius;

        #region public getter
        public float ToggleRadius { get => toggleRadius; }
        public ShotgunPerk[] ShotgunPerks { get => shotgunPerks; }
        public RiflePerk[] RiflePerks { get => riflePerks;}
        public SniperPerk[] SniperPerks { get => sniperPerks; }
        public LasergunPerk[] LasergunsPerk { get => lasergunsPerk; }
        #endregion

    }
}