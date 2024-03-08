using UnityEngine;

namespace Bioweapon
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
    public class GameManagerScriptableObject : ScriptableObject
    {
        [SerializeField] private float timePassPerTurn;

        public float TimePassPerTurn { get => timePassPerTurn; }
    }
}