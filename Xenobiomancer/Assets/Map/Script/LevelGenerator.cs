using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    int maxDepth;

    Graph graph;

    void Initialize()
    {

    }

    void CreateNode(int depth)
    {
        LevelNode node = new();
        node.Id = graph.NodeCount;
        node.Depth = depth;
        graph.AddNode(node);
    }

    void CreateSpawnNode()
    {

    }

    void CreateRootNode()
    {

    }
}
