using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
using Patterns;
public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    int maxDepth, maxNodesPerDepth, maxStartRooms;
    [SerializeField]
    float offX, offY, spacingY, spacingX;
    [SerializeField]
    RectTransform boxTransform;
    [SerializeField]
    GameObject masterNodePrefab,nodePrefab,linePrefab;

    MapGraph graph;
    Dictionary<NodeEncounter, float> encounterProbability;

    private void Start()
    {
        encounterProbability = new();
        encounterProbability.Add(NodeEncounter.INFESTED, 2);
        encounterProbability.Add(NodeEncounter.ABANDONED, 1);
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
        AddMasterNode();
        DisplayMap();
        CheckIfMapConnected();

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
                node.EncounterType = GetRandomEncounter();
                if (depth == 0)
                    node.EncounterType = NodeEncounter.INFESTED;
                graph.AddNode(node);
                
                //loading flag

            }
        }
    }

    /// <summary>
    /// Get a random encounter using the weighted probability manager
    /// </summary>
    NodeEncounter GetRandomEncounter()
    {
        return ProbabilityManager.SelectWeightedItem(encounterProbability);
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

    /// <summary>
    /// Check if the map is a connected graph. If not, it will
    /// do some action to make it a connected graph.
    /// </summary>
    void CheckIfMapConnected()
    {
        if (graph.IsConnectedGraph())
        {
            return;
        }

        //finish up this part
        //incomplete
        List<MapNode> startingRooms = GetStartingRooms();
        int minCount = 999999999;
        Node detachedPathStartNode;

        foreach (MapNode node in startingRooms)
        {
            List<MapNode> path = new();
            graph.Search(node, path);

            if(path.Count < minCount)
            {
                minCount = path.Count;
                detachedPathStartNode = node;
            }
        }

        
    }

    /// <summary>
    /// Check the entire node list and removes node / edges that
    /// does not satisfy conditions that we set
    /// </summary>
    void PruneMap()
    {
        MapNode[] nodeList = new MapNode[graph.NodeList.Count];
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

    #region DISPLAYING MAP
    void DisplayMap()
    {
        foreach (MapNode node in graph.NodeList)
        {
            if(node.EncounterType == NodeEncounter.BOSS)
            {
                DisplayMasterNode(node);
            }
            else
            {
                DisplayNode(node);
            }

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
        node.Object.SetSprite();
        if (node.Depth == 0)
            node.Object.MakeAccessible();
    }

    void DisplayMasterNode(MapNode node)
    {
        //spawn a master node from a prefab
        var go = Instantiate(masterNodePrefab, boxTransform).GetComponent<RectTransform>();
        go.gameObject.name = $"MasterNode";
        go.localPosition = node.Position;
        go.localScale = new(1, 1, 1);
        go.GetComponent<NodeObject>().Node = node;
        node.Object = go.GetComponent<NodeObject>();
    }

    void DisplayPath(MapNode node, MapNode target)
    {
        //spawn a line
        var line = Instantiate(linePrefab, boxTransform).GetComponent<LineRenderer>();
        line.GetComponent<RectTransform>().localPosition = new(0, 0);
        line.useWorldSpace = false;
        line.SetPosition(0, node.Position + Vector3.back);
        line.SetPosition(1, target.Position + Vector3.back);
    }
    #endregion

    /// <summary>
    /// Disable nodes in the same depth as the node provided
    /// </summary>
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

    /// <summary>
    /// Makes the connected node in the next depth accessible
    /// </summary>
    void ConnectedNodeAccessible(Node node)
    {
        List<MapNode> nodesConnected = graph.GetConnectedNextDepth(node.Id);
        foreach (MapNode nodeConnected in nodesConnected)
        {
            nodeConnected.Object.MakeAccessible();
        }
    }

    void AddMasterNode()
    {
        int depth = maxDepth;
        float x = -200;
        float y = offY - 150 + depth * spacingY;

        MapNode node = new();
        node.Id = graph.NodeCount;
        node.Depth = depth;
        node.IndexInDepth = 0;
        node.Position = new(x, y);
        node.EncounterType = NodeEncounter.BOSS;
        graph.AddNode(node);

        //connect master node to end depth
        List<MapNode> nodeAtEndDepth = graph.GetNodesInDepth(maxDepth - 1);

        foreach (var nodeEnd in nodeAtEndDepth)
        {
            nodeEnd.AdjacencyList.Add(node);
            node.ConnectedNodesPrevDepth.Add(nodeEnd);
            graph.AddEdge(nodeEnd.Id, node);
        }
    }
}
