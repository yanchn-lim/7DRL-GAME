using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Room Data", menuName = "Room Data")]
public class RoomData : ScriptableObject
{
    public int Width, Height;
    public Vector2Int Center;
    public TileBase[] Tiles;
    public RoomName Name;

    public Vector2Int GetRoomCenter()
    {
        Center = new(Width/2,Height/2);
        return Center;
    }

    struct TileData
    {
        public string Name;
        public TileBase Tile;
    }
}

public enum RoomName
{
    SPAWN,
    ROOM_1,
    ROOM_2,
    ROOM_3,
    ROOM_4,
    ROOM_5
}