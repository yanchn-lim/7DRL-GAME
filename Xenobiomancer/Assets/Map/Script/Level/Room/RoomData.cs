using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Room Data", menuName = "Room Data")]
public class RoomData : ScriptableObject
{
    public int Width, Height;
    public TileBase GroundTiles;
    public TileBase WallTiles;
    public TileBase[] Tiles;


    enum EntranceDirection
    {
        NORTH,
        SOUTH,
        EAST,
        WEST
    }
}