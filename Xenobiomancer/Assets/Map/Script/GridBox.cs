using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridBox
{
    public Vector2 GridSize;
    public Vector2 Position;
}

public class DisplayGridBox : MonoBehaviour
{
    public GridBox box;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(box.Position,box.GridSize);
    }
}