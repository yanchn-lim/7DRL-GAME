using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;

public class MapGraph : Graph
{
    public new List<MapNode> GetNodesInDepth(int depth)
    {
        List<MapNode> NodesInDepth = new();

        foreach (MapNode node in NodeList) //looping the list of nodes
        {
            if (node.Depth == depth)//checking depth
            {
                NodesInDepth.Add(node);
            }
        }

        return NodesInDepth;
    }

    public new List<MapNode> GetConnectedNextDepth(int id)
    {
        List<MapNode> nextDepth = new();
        if (AdjacencyList.ContainsKey(id))//searches the list and returns a list of nodes connected
        {
            foreach (MapNode node in AdjacencyList[id])
            {
                if (node.Depth == GetNode(id).Depth + 1)
                {
                    nextDepth.Add(node);
                }
            }
            return nextDepth;
        }

        return null;
    }
}
