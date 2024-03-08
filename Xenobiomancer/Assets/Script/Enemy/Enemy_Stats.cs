using UnityEngine;

namespace enemySS
{
    [CreateAssetMenu(fileName = "Enemy_status", menuName = "ScriptableObjects/Enemy_Stats")]
    public class Enemy_Stats : ScriptableObject
    {
        public string Type;

        public float max_Health;
        public float min_Health;
        public float current_Health;
        public Vector3[] spawnPoints;
        public float damageToPlayer;
        public float protectionLevel;
        public float RangeofSight;
        public float MovementSpeed;
        public float max_Protection;


    }
}