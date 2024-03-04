using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
using Patterns;
public class NodeMapGenerator : MonoBehaviour
{
    [SerializeField]
    int MaxDepth,MinNodesPerDepth, MaxNodesPerDepth, MinStartRooms, MaxStartRooms;
    [SerializeField]
    int GridSize;
    [SerializeField]
    RectTransform boxTransform;

    Rect box;
    Graph graph;
    [SerializeField]
    GameObject nodePrefab,linePrefab;
    private void Start()
    {
        InitializeMap();
    }

    void InitializeMap()
    {
        graph = new();
        box = boxTransform.rect;

        //move to start of game
        RandSeedManager.GenerateSeed();

        //GENERATE NODES
        //GenerateNodes();
        GenerateNodeGrid();
        GeneratePath();

        //GENERATE THE EDGES
        //GenerateEdges();


        //DISPLAY THE NODE + EDGES

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

    void GenerateGrid()
    {
        int xNum = Mathf.FloorToInt(box.width/GridSize);
        int yNum = Mathf.FloorToInt(box.height/GridSize);
        float halfSize = GridSize / 2;

        for (int x = 0; x < xNum; x++)
        {
            for (int y = 0; y < yNum; y++)
            {
                float xPos = x * GridSize;
                float yPos = -y * GridSize;

                RectTransform cellRect = new GameObject($"Cell_{x}_{y}").AddComponent<RectTransform>();
                cellRect.SetParent(boxTransform);
                cellRect.anchorMin = new Vector2(0, 1);
                cellRect.anchorMax = new Vector2(0, 1);
                cellRect.pivot = new Vector2(0, 1);
                cellRect.anchoredPosition = new Vector2(xPos, yPos);
                cellRect.localScale = new(1, 1, 1);
                cellRect.localRotation = Quaternion.Euler(0,0,0) ;

            }
        }
    }

    void GenerateNodeGrid()
    {
        for (int depth = 0; depth < MaxDepth; depth++)
        {
            for (int i = 0; i < MaxNodesPerDepth; i++)
            {
                float y = offY + depth * spacingY;
                float x = offX + i * spacingX;

                //generate nodes here
                Node node = new();
                node.Id = graph.NodeCount;
                node.Depth = depth;
                node.IndexInDepth = i;
                node.Position = new(x, y);

                var go = Instantiate(nodePrefab, boxTransform).GetComponent<RectTransform>();
                go.gameObject.name = $"Node-{depth}-D-{i}";
                go.localPosition = node.Position;
                go.localScale = new(1, 1, 1);

                graph.AddNode(node);
                //loading flag
            }
        }
    }

    List<Node> GetStartingRooms()
    {
        Debug.Log("getting starting rooms");
        List<Node> nodesAtStart = graph.GetNodesInDepth(0);
        List<Node> startingNodes = new();
        //int numRooms = Random.Range(MinStartRooms, MaxStartRooms + 1);

        for (int i = 0; i < MaxStartRooms; i++)
        {
            int randIndex = Random.Range(0, nodesAtStart.Count);
            Node selectedNode = nodesAtStart[randIndex];
            if (!startingNodes.Contains(selectedNode))
            {
                startingNodes.Add(selectedNode);
                Debug.Log("adding node");
            }
        }

        return startingNodes;
    }

    void GeneratePath()
    {
        Debug.Log("generating paths");
        List<Node> startingNodes = GetStartingRooms();
        Debug.Log(startingNodes.Count);
        foreach (var node in startingNodes)
        {
            int index = node.IndexInDepth;
            WalkConnections(node);
        }
    }

    void WalkConnections(Node node)
    {
        Debug.Log($"walking : node_{node.Id}");
        if(node.Depth == MaxDepth - 1)
        {
            //connect to master node here?
            return;
        }

        List<Node> nextDepth = graph.GetNodesInDepth(node.Depth + 1);
        int indexL = node.IndexInDepth - 1;
        int indexM = node.IndexInDepth;
        int indexR = node.IndexInDepth + 1;

        if (indexL < 0)
            indexL = 0;
        if (indexR > MaxNodesPerDepth - 1)
            indexR = MaxNodesPerDepth - 1;

        int index = Random.Range(0, 3);
        Node selectedNode;
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
        node.AdjacencyList.Add(selectedNode);
        selectedNode.ConnectedNodesPrevDepth.Add(node);

        var line = Instantiate(linePrefab, boxTransform).GetComponent<LineRenderer>();
        line.GetComponent<RectTransform>().localPosition = new(0,0);
        line.useWorldSpace = false;
        line.SetPosition(0, node.Position+Vector3.back);
        line.SetPosition(1, selectedNode.Position+Vector3.back);
        WalkConnections(selectedNode);
    }

    [SerializeField]
    float offX, offY,spacingY,spacingX;

    void GenerateNodes()
    {
        for (int depth = 0; depth < MaxDepth; depth++)
        {
            int numNodes = Random.Range(MinNodesPerDepth, MaxNodesPerDepth + 1);

            for (int j = 0; j < numNodes; j++)
            {
                float y = offY + depth * spacingY;
                float x = offX + j * spacingX;

                //generate nodes here
                Node node = new();
                node.Id = graph.NodeCount;
                node.Depth = depth;
                node.Position = new(x, y);

                var go = new GameObject($"Node-{node.Id}-D-{depth}").AddComponent<RectTransform>();
                go.gameObject.AddComponent<SpriteRenderer>();
                go.SetParent(boxTransform);
                go.localPosition = node.Position;
                go.localScale = new(1,1,1);

                if (depth == 0)
                {
                    node.EncounterType = NodeEncounter.INFESTED;
                    node.IsAccesible = true;
                }
                else
                {
                    //randomly select node encounter here
                    node.EncounterType = NodeEncounter.INFESTED;
                    node.IsAccesible = false;
                }

                graph.AddNode(node);

                //add a flag for loading screen here
                //
            }
        }
    }

    void GenerateEdges()
    {
        for (int depth = 0; depth < MaxDepth; depth++)
        {
            List<Node> nodesInDepth = graph.GetNodesInDepth(depth);
            List<Node> nodesInNextDepth = null;
            if (depth < MaxDepth - 1)
            {
                nodesInNextDepth = graph.GetNodesInDepth(depth + 1);
            }

            int count = nodesInDepth.Count;
            int countND = nodesInNextDepth.Count;

            for (int i = 0; i < count; i++)
            {
                Node currNode = nodesInDepth[i];
                if (nodesInNextDepth == null)
                {
                    //connect to master node

                    continue;
                }

                Dictionary<int, float> weightedConnection = new();



                //add flag for loading

            }
        }

    }

}
