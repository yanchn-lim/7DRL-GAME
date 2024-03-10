using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GridHelper : Patterns.Singleton<GridHelper>
{
    [SerializeField] Tilemap grid; //the obstacle that exist in the game
    [SerializeField] Tilemap obstacle; //

    

    //https://www.redblobgames.com/pathfinding/a-star/introduction.html
    /// <summary>
    /// Find the shortest path from one end to the other
    /// </summary>
    /// <param name="starting">starting position of the point</param>
    /// <param name="ending">ending position of the point</param>
    /// <returns>Path in the form of stack</returns>
    public Stack<Vector2> GeneratePath(Vector2 starting, Vector2 ending)
    {
        Vector2Int startPoint = (Vector2Int)grid.WorldToCell(starting);
        Vector2Int endPoint = (Vector2Int)grid.WorldToCell(ending);

        PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> costSoFar = new Dictionary<Vector2Int, float>();

        frontier.Enqueue(startPoint, 0);
        
        costSoFar.Add(startPoint, 0f);

        while( frontier.Count > 0 )
        {
            var current = frontier.Dequeue().Item1;
            if (current == endPoint) break;

            foreach(var cell in GetCellsAroundNeighbour(current))
            {
                float newCost = costSoFar[current] + 1;//1 is a constant

                if (!costSoFar.ContainsKey(cell) )
                {
                    costSoFar[cell] = newCost;
                    float priority = newCost + Heuristic(endPoint, cell);
                    frontier.Enqueue(cell, priority);
                    cameFrom.Add(cell, current);
                }
                //if contain key then check the new cost is lower than the 
                else if(newCost < costSoFar[cell])
                {
                    costSoFar[cell] = newCost;
                    float priority = newCost + Heuristic(endPoint, cell);
                    frontier.Enqueue(cell, priority);
                    if (cameFrom.ContainsKey(cell))
                    {
                        cameFrom[cell] = current; //replace with the new one
                    }
                    else
                    {
                        cameFrom.Add(cell, current); //error here
                    }
                }
            }

        }

        Vector2Int currentPoint = endPoint;
        Stack<Vector2> path = new Stack<Vector2>();
        while( currentPoint != startPoint )
        { 
            path.Push((Vector2)grid.GetCellCenterWorld((Vector3Int)currentPoint));
            currentPoint = cameFrom[currentPoint];
        }

        //Stack< Vector2 > debugDummy = new Stack<Vector2 >(path);
        //Vector2 previous = debugDummy.Pop();
        //while (debugDummy.Count > 0)
        //{
        //    Vector2 position = debugDummy.Pop();
        //    Debug.DrawLine(previous, position , UnityEngine.Color.yellow);
        //    previous = position;
        //}

        return path;
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    //private float CostToTravelGrid(Vector2Int Destination)
    //{
    //    if(obstacle.GetTile((Vector3Int) Destination) == null)
    //    {
    //        return 1f;
    //    }
    //    else
    //    {
    //        //the weight to travel to the offset grid
    //        return 3f;
    //    }
    //}

    private Vector2Int[] GetCellsAroundNeighbour(Vector2Int cell)
    {
        List<Vector2Int> output = new List<Vector2Int>();
        Vector2Int point = new Vector2Int(cell.x + 1, cell.y + 1);
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if (point == cell)
                {
                    point = new Vector2Int(point.x - 1, point.y );
                    continue;
                }
                //verify whether the cell can be used

                var tile= grid.GetTile((Vector3Int)point);
                var obstacleTile = obstacle.GetTile((Vector3Int)point);
                //print($"raycast hit at{worldPoint}");
                if (tile == null && obstacleTile == null)
                {
                    //if there is nothing
                    output.Add(point); 
                }
                //move to the next point
                point = new Vector2Int(point.x - 1, point.y);
            }
            //going to have -3x

            point = new Vector2Int(point.x + 3, point.y -1 );
        }
        return output.ToArray();
    }

}

