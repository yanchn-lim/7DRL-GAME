using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity;
using System;

[CreateAssetMenu(fileName = "New Room Data", menuName = "Room Data")]
public class RoomData : ScriptableObject
{
    public int Width, Height;
    public Vector2Int Center;
    public TileBase[] Tiles;

    public Vector2Int GetRoomCenter()
    {
        Center = new(Width/2 + 1,Height/2 + 1);
        return Center;
    }
}
