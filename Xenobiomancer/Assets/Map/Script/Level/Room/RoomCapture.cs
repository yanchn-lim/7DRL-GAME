using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
public class RoomCapture : MonoBehaviour
{
    public Tilemap roomTileMap;
    public RoomData roomData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CaptureRoomData();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            roomTileMap.ClearAllTiles();
            RoomGenerator.GenerateRoom(roomTileMap,roomData,new(0,0,0));
        }

    }

    public RoomData CaptureRoomData()
    {
        roomTileMap.CompressBounds();
        BoundsInt bounds = roomTileMap.cellBounds;
        TileBase[] capturedTiles = roomTileMap.GetTilesBlock(bounds);
        roomData.Tiles = capturedTiles;
        roomData.Width = bounds.size.x;
        roomData.Height = bounds.size.y;
    
        roomData.GetRoomCenter();
        EditorUtility.SetDirty(roomData);
        Debug.Log("created room");
        return roomData;
    }

}
 