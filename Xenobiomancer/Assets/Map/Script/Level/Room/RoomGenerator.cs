using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField]
    Tilemap tileMap;
    [SerializeField]
    RoomData data;

    private void Start()
    {
        GenerateRoom(data,new(0,0,0));
    }

    public void GenerateRoom(RoomData data, Vector3Int pos)
    {
        for (int x = 0; x < data.Width; x++)
        {
            for (int y = 0; y < data.Height; y++)
            {
                Vector3Int tilePos = new(pos.x + x, pos.y + y, pos.z);

                TileBase tile = data.Tiles[x + y * data.Width];
                //edit the room
                //code here

                if (tile != null)
                {
                    tileMap.SetTile(tilePos, tile);
                }
            }
        }
    }

    public static void GenerateRoom(Tilemap map,RoomData data, Vector3Int pos)
    {
        for (int x = 0; x < data.Width; x++)
        {
            for (int y = 0; y < data.Height; y++)
            {
                int offsetX = data.Center.x;
                int offsetY = data.Center.y;
                Vector3Int tilePos = new(pos.x + x - offsetX, pos.y + y - offsetY, pos.z);

                TileBase tile = data.Tiles[x + y * data.Width];
                //edit the room
                //code here
                if (tile != null && map.GetTile(tilePos) == null)
                {
                    map.SetTile(tilePos, tile);
                }
            }
        }
    }
}
