using UnityEngine;

namespace UpgradeStation
{
    [CreateAssetMenu(fileName = "upgradestationData", menuName = "ScriptableObjects/Upgrade Station")]
    public class UpgradeStationData : ScriptableObject
    {
        [Tooltip("What perks the upgrade station have")]
        [SerializeField] private Perk[] availablePerks;
        [Tooltip("radius the player should be in to toggle the upgrade station")]
        [SerializeField] private float toggleRadius;

        public Perk[] AvailablePerks { get => availablePerks; }
        public float ToggleRadius { get => toggleRadius; }
    }
}