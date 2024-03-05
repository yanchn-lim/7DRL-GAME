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
    public new bool IsConnectedGraph()
    {
        System.Diagnostics.Stopwatch time = new();
        time.Start();
        List<MapNode> visited = new();

        List<MapNode> nodeFirstDepth = GetNodesInDepth(0);
        int randIndex = Random.Range(0, nodeFirstDepth.Count);
        Search(nodeFirstDepth[randIndex], visited);

        Debug.Log($"VISITED : {visited.Count} \nADJACENTLIST : {AdjacencyList.Count -1}");
        Debug.Log($"TIME ELAPSED : {time.ElapsedMilliseconds}");
        return visited.Count == AdjacencyList.Count - 1;
    }
    
    public void Search(MapNode node, List<MapNode> visited)
    {
        if(node.EncounterType == NodeEncounter.BOSS)
            return;

        if (!visited.Contains(node))
        {
            visited.Add(node);

            if (AdjacencyList.TryGetValue(node.Id, out List<Node> neighbour))
            {
                foreach (var n in neighbour)
                {
                    Search((MapNode)n, visited);
                }
            }
        }
    }
}
