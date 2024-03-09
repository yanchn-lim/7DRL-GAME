using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public int Width;
    public int Height;
    public List<TileData> LevelTiles = new(); 

    public class TileData
    {
        public TileBase Tile;
        public Sprite Sprite;
        public Vector3Int Position;
        public TileType Type;
    }

    public enum TileType
    {
        WALL,
        FLOOR,
        UPGRADE
    }
}
