using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_status", menuName = "ScriptableObjects/Enemy_Stats")]
public class Enemy_Stats : ScriptableObject
{
    // storing the data for the enemies
    public string Type;

    public float max_Health;
    public float min_Health;
    public Vector3[] spawnPoints;
    public float damageToPlayer;
    public float RangeofSight;
    public float MovementSpeed;
    public float max_Protection;
    public float min_Protection;

    
}
