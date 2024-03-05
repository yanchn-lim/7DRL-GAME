using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
using Patterns;
public class NodeMapGenerator : MonoBehaviour
{
    [SerializeField]
    int maxDepth, maxNodesPerDepth, maxStartRooms;
    [SerializeField]
    float offX, offY, spacingY, spacingX;
    [SerializeField]
    RectTransform boxTransform;
    [SerializeField]
    GameObject nodePrefab,linePrefab;

    MapGraph graph;

    private void Start()
    {
        EventManager.Instance.AddListener<Node>(EventName.MAP_NODE_CLICKED, DisableNodesInDepth);
        EventManager.Instance.AddListener<Node>(EventName.MAP_NODE_CLICKED, ConnectedNodeAccessible);
        InitializeMap();
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<Node>(EventName.MAP_NODE_CLICKED, DisableNodesInDepth);
        EventManager.Instance.RemoveListener<Node>(EventName.MAP_NODE_CLICKED, ConnectedNodeAccessible);
    }

    void InitializeMap()
    {
        graph = new();

        //move to start of game
        RandSeedManager.GenerateSeed();

        //GENERATE NODES
        GenerateNodeGrid();
        GeneratePath();
        PruneMap();
        DisplayMap();
        CheckIfMapConnected();

        //debugging
        Debugging();
    }

    void Debugging()
    {
        //for (int i = 0; i < graph.NodeCount; i++)
        //{
        //    Debug.Log(graph.NodeList[i].Id + ", " + graph.NodeList[i].Depth);
        //}
    }

    ///<summary>
    ///Generate a grid of nodes without paths
    ///</summary>
    void GenerateNodeGrid()
    {
        for (int depth = 0; depth < maxDepth; depth++)
        {
            for (int i = 0; i < maxNodesPerDepth; i++)
            {
                float y = offY + depth * spacingY;
                float x = offX + i * spacingX;

                //generate nodes here
                MapNode node = new();
                node.Id = graph.NodeCount;
                node.Depth = depth;
                node.IndexInDepth = i;
                node.Position = new(x, y);               
                graph.AddNode(node);

                //loading flag

            }
        }
    }

    ///<summary>
    ///Randomly get starting nodes from the starting depth of 0
    ///</summary>
    List<MapNode> GetStartingRooms()
    {
        List<MapNode> nodesAtStart = graph.GetNodesInDepth(0);
        List<MapNode> startingNodes = new();

        for (int i = 0; i < maxStartRooms; i++)
        {
            int randIndex = Random.Range(0, nodesAtStart.Count);
            MapNode selectedNode = nodesAtStart[randIndex];
            if (!startingNodes.Contains(selectedNode))
            {
                startingNodes.Add(selectedNode);
            }
        }

        //flag loading

        return startingNodes;
    }

    ///<summary>
    ///For each of the starting nodes, it will walk a path
    ///towards the end depth
    ///</summary>
    void GeneratePath()
    {
        List<MapNode> startingNodes = GetStartingRooms();

        //walk the path from the starting node to the final depth
        foreach (var node in startingNodes)
        {
            int index = node.IndexInDepth;
            WalkConnections(node);
        }
    }

    ///<summary>
    ///For each of the starting nodes, it will walk a path
    ///towards the end depth
    ///</summary>
    void WalkConnections(MapNode node)
    {
        if(node.Depth == maxDepth - 1)
        {
            //connect to master node here?
            return;
        }

        List<MapNode> nextDepth = graph.GetNodesInDepth(node.Depth + 1);

        //possible connections
        int indexL = Mathf.Clamp(node.IndexInDepth - 1,0,maxNodesPerDepth-1);
        int indexM = node.IndexInDepth;
        int indexR = Mathf.Clamp(node.IndexInDepth + 1, 0, maxNodesPerDepth - 1);

        int index = Random.Range(0, 3);
        MapNode selectedNode;

        //get the selected node from the random index
        //by default it would be the left node
        switch (index)
        {
            case 0:
                selectedNode = nextDepth[indexL];
                break;
            case 1:
                selectedNode = nextDepth[indexM];
                break;
            case 2:
                selectedNode = nextDepth[indexR];
                break;
            default:
                selectedNode = nextDepth[indexL];
                break;
        }

        //add it to this node's adjacency list
        node.AdjacencyList.Add(selectedNode);
        selectedNode.ConnectedNodesPrevDepth.Add(node);
        graph.AddEdge(node.Id,selectedNode);

        //continue walking from the selected node
        WalkConnections(selectedNode);
    }

    void CheckIfMapConnected()
    {
        if (graph.IsConnectedGraph())
        {
            return;
        }

        List<MapNode> startingRooms = GetStartingRooms();
        int minCount = 999999999;
        Node detachedPathStartNode;

        foreach (var node in startingRooms)
        {
            List<Node> path = new();
            graph.Search(node, path);

            if(path.Count < minCount)
            {
                minCount = path.Count;
                detachedPathStartNode = node;
            }
        }

        
    }

    void PruneMap()
    {
        Node[] nodeList = new Node[graph.NodeList.Count];
        graph.NodeList.CopyTo(nodeList);

        foreach (var node in nodeList)
        {
            if(node.AdjacencyList.Count == 0)
            {
                if(node.Depth == maxDepth - 1 && node.ConnectedNodesPrevDepth.Count != 0)
                {
                    continue;
                }
                graph.RemoveNode(node);
            }
        }
    }

    void DisplayMap()
    {
        foreach (MapNode node in graph.NodeList)
        {
            DisplayNode(node);

            foreach (MapNode target in node.AdjacencyList)
            {
                DisplayPath(node, target);
            }
        }
    }

    void DisplayNode(MapNode node)
    {
        var go = Instantiate(nodePrefab, boxTransform).GetComponent<RectTransform>();
        go.gameObject.name = $"Node-{node.Depth}-D-{node.IndexInDepth}";
        go.localPosition = node.Position;
        go.localScale = new(1, 1, 1);        
        go.GetComponent<NodeObject>().Node = node;
        node.Object = go.GetComponent<NodeObject>();
        if (node.Depth == 0)
            node.Object.MakeAccessible();
    }

    void DisplayPath(MapNode node, MapNode target)
    {
        //for display
        var line = Instantiate(linePrefab, boxTransform).GetComponent<LineRenderer>();
        line.GetComponent<RectTransform>().localPosition = new(0, 0);
        line.useWorldSpace = false;
        line.SetPosition(0, node.Position + Vector3.back);
        line.SetPosition(1, target.Position + Vector3.back);
    }

    void DisableNodesInDepth(Node node)
    {
        List<MapNode> nodesInDepth = graph.GetNodesInDepth(node.Depth);
        foreach (MapNode nodeInDepth in nodesInDepth)
        {
            if (nodeInDepth.Id == node.Id)
            {
                continue;
            }
            nodeInDepth.Object.MakeInAccessible();
        }
    }

    void ConnectedNodeAccessible(Node node)
    {
        List<MapNode> nodesConnected = graph.GetConnectedNextDepth(node.Id);
        foreach (MapNode nodeConnected in nodesConnected)
        {
            nodeConnected.Object.MakeAccessible();
        }
    }

}
