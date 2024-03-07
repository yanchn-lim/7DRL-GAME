using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
using Patterns;
using UnityEngine.Tilemaps;
public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    int minDepth,maxDepth,maxHorizontalDepth;
    [SerializeField]
    GameObject spawnPrefab, rootPrefab, nodePrefab, linePrefab;
    [SerializeField]
    RoomData spawnRoomData;
    [SerializeField]
    RoomData[] roomData;
    [SerializeField]
    Tilemap tileMap;

    int currMaxDepth;

    LevelGraph graph;
    LevelNode rootNode;
    LevelNode spawnNode;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ProbabilityManager.TestProbability();
        }
    }

    void Initialize()
    {
        graph = new();

        CreateStart();
        CreateSpine();
        CreateBranch();
        AssignPos();
        AssignRoom();
        GenerateRoom();
        //Debugging();

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

    LevelNode CreateNode(int depth, int horizontalDepth, LevelNodeType type)
    {
        LevelNode node = new();
        node.Id = graph.NodeCount;
        node.Depth = depth;
        node.HorizontalDepth = horizontalDepth;
        node.Type = type;
        graph.AddNode(node);
        return node;
    }

    void CreateStart()
    {
        spawnNode = CreateNode(-1, 0, LevelNodeType.SPAWN);
        rootNode = CreateNode(0, 0, LevelNodeType.ROOT);
        graph.AddEdge(spawnNode.Id, rootNode);
    }

    void CreateSpine()
    {
        LevelNode prevNode = rootNode;
        currMaxDepth = Random.Range(minDepth, maxDepth);

        for (int i = 1; i < currMaxDepth + 1; i++)
        {
            LevelNode currNode = CreateNode(i,0,LevelNodeType.NORMAL);
            graph.AddEdge(prevNode, currNode);
            prevNode = currNode;
        }
    }

    #region Creating Branches
    public float k = 0.1f;
    void CreateBranch()
    {
        int halfDepth = Mathf.FloorToInt(currMaxDepth/2);

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
            LevelNode currNode = CreateNode(rootNode.Depth, -i, LevelNodeType.NORMAL);
            graph.AddEdge(prevNode, currNode);
            prevNode = currNode;
        }
    }

    void CreateRightBranch(int num, LevelNode rootNode)
    {
        LevelNode prevNode = rootNode;
        for (int i = 1; i < num + 1; i++)
        {
            LevelNode currNode = CreateNode(rootNode.Depth, i, LevelNodeType.NORMAL);
            graph.AddEdge(prevNode, currNode);
            prevNode = currNode;
        }
    }

    float GetWeight(float weight,float i)
    {
        float w = 1 - Mathf.Pow(i - (weight + (i * k)), 3);
        return Mathf.Abs(w);
    }
    #endregion

    void AssignRoom()
    {
        foreach (var node in graph.NodeList)
        {
            if(node.Type == LevelNodeType.SPAWN)
            {
                node.RoomData = spawnRoomData;
                continue;
            }

            int randIndex = Random.Range(0,roomData.Length);
            node.RoomData = roomData[randIndex];

        }
    }

    void GenerateRoom()
    {
        foreach (var node in graph.NodeList)
        {
            RoomGenerator.GenerateRoom(tileMap,node.RoomData,node.Position);
        }
    }

    #region DEBUGGING
    void Debugging()
    {
        //for visualization
        AssignPos();
        DisplayMap();
    }

    void AssignPos()
    {
        int offsetY = 10;
        int offsetX = 10;
        foreach (LevelNode item in graph.NodeList)
        {
            int y = item.Depth * offsetY;
            int x = item.HorizontalDepth * offsetX;
            Vector3Int pos = new(x, y,0);
            item.Position = pos;
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

        go.position = node.Position;
    }

    void DisplayPath(LevelNode node, LevelNode target)
    {
        //spawn a line
        var line = Instantiate(linePrefab).GetComponent<LineRenderer>();
        line.SetPosition(0, node.Position + Vector3.back);
        line.SetPosition(1, target.Position + Vector3.back);
    }

    #endregion
}
