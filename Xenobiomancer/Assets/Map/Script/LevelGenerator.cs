using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
using Patterns;
public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    int minDepth,maxDepth,maxHorizontalDepth;
    [SerializeField]
    GameObject spawnPrefab, rootPrefab, nodePrefab, linePrefab;
    [SerializeField]
    AnimationCurve curve;
    int currMaxDepth;

    LevelGraph graph;
    LevelNode rootNode;
    LevelNode spawnNode;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        graph = new();

        CreateStart();
        CreateSpine();
        CreateBranch();
        Debugging();

    }

    LevelNode CreateNode(int depth,int horizontalDepth)
    {
        LevelNode node = new();
        node.Id = graph.NodeCount;
        node.Depth = depth;
        node.HorizontalDepth = horizontalDepth;
        graph.AddNode(node);
        return node;
    }

    #region Initialize Start of level
    void CreateStart()
    {
        CreateSpawnNode();
        CreateRootNode();
        ConnectSpawnToRoot();
    }

    void CreateSpawnNode()
    {
        spawnNode = CreateNode(-1,0);
        spawnNode.Type = LevelNodeType.SPAWN;
    }

    void CreateRootNode()
    {
        rootNode = CreateNode(0,0);
        rootNode.Type = LevelNodeType.ROOT;
    }

    void ConnectSpawnToRoot()
    {
        graph.AddEdge(spawnNode.Id,rootNode);
    }
    #endregion

    void CreateSpine()
    {
        LevelNode prevNode = rootNode;
        currMaxDepth = Random.Range(minDepth, maxDepth);

        for (int i = 1; i < currMaxDepth + 1; i++)
        {
            LevelNode currNode = CreateNode(i,0);
            currNode.Type = LevelNodeType.NORMAL;
            graph.AddEdge(prevNode, currNode);
            prevNode = currNode;
        }
    }
    public float k = 0.1f;
    void CreateBranch()
    {
        int halfDepth = Mathf.FloorToInt(currMaxDepth/2);

        //set graph
        int maxWeight = Mathf.Abs(graph.MaxDepth - halfDepth);

        foreach (var node in graph.GetSpine())
        {
            if (node.Depth == -1)
                continue;

            float weight = Mathf.Abs(node.Depth - halfDepth);
            node.DebugWeight = weight;
            Dictionary<int, float> numRoomWeight = new();
            float totalW = 0;
            for (int i = 0; i < maxHorizontalDepth + 1; i++)
            {
                float w = GetWeight(weight,i);
                numRoomWeight.Add(i, w);
                totalW += w;
            }

            for (int i = 0; i < maxHorizontalDepth + 1; i++)
            {
                float w = GetWeight(weight, i);
                Debug.Log($"Weight : {weight} HoriDepth : {i} % = {w/totalW * 100}% W = {w}");
            }

            int numRoomsL = ProbabilityManager.SelectWeightedItem(numRoomWeight);
            int numRoomsR = ProbabilityManager.SelectWeightedItem(numRoomWeight);

            CreateLeftBranch(numRoomsL, node);
            CreateRightBranch(numRoomsR, node);

        }
        
    }

    void CreateLeftBranch(int num,LevelNode rootNode)
    {
        LevelNode prevNode = rootNode;
        for (int i = 1; i < num + 1; i++)
        {
            LevelNode currNode = CreateNode(rootNode.Depth, -i);
            currNode.Type = LevelNodeType.NORMAL;
            graph.AddEdge(prevNode, currNode);
            prevNode = currNode;
        }
    }

    void CreateRightBranch(int num, LevelNode rootNode)
    {
        LevelNode prevNode = rootNode;
        for (int i = 1; i < num + 1; i++)
        {
            LevelNode currNode = CreateNode(rootNode.Depth, i);
            currNode.Type = LevelNodeType.NORMAL;
            graph.AddEdge(prevNode, currNode);
            prevNode = currNode;
        }
    }

    float GetWeight(float weight,float i)
    {
        float w = 1 - Mathf.Pow(i - (weight + (i * k)), 3);
        return Mathf.Abs(w);
    }

    void Debugging()
    {
        //for visualization
        AssignPos();
        DisplayMap();
    }

    void AssignPos()
    {
        float offsetY = 1.2f;
        float offsetX = 1.2f;
        foreach (LevelNode item in graph.NodeList)
        {
            float y = item.Depth * offsetY;
            float x = item.HorizontalDepth * offsetX;
            Vector2 pos = new(x, y);
            item.DebugPos = pos;
        }
    }

    void DisplayMap()
    {
        foreach (LevelNode node in graph.NodeList)
        {
            switch (node.Type)
            {
                case LevelNodeType.ROOT:
                    DisplayNode(node, rootPrefab);

                    break;
                case LevelNodeType.SPAWN:
                    DisplayNode(node, spawnPrefab);

                    break;
                case LevelNodeType.NORMAL:
                    DisplayNode(node, nodePrefab);

                    break;
                default:
                    Debug.Log("node type defaulted");
                    break;
            }

            foreach (LevelNode target in node.AdjacencyList)
            {
                DisplayPath(node, target);
            }
        }
  
    }

    void DisplayNode(LevelNode node,GameObject prefab)
    {
        var go = Instantiate(prefab).transform;
        go.gameObject.name = $"Node-{node.Depth}";
        node.DebugColor = go.GetComponent<SpriteRenderer>().color;
        Color c = node.DebugColor;
        if (node.Depth != -1 && node.DebugWeight != -1)
        {
            c /= node.DebugWeight;
            c.a = 1;
            node.DebugColor = c;
            go.GetComponent<SpriteRenderer>().color = c;
        }

        go.position = node.DebugPos;
    }

    void DisplayPath(LevelNode node, LevelNode target)
    {
        //spawn a line
        var line = Instantiate(linePrefab).GetComponent<LineRenderer>();
        line.SetPosition(0, node.DebugPos + Vector3.back);
        line.SetPosition(1, target.DebugPos + Vector3.back);
    }
}
