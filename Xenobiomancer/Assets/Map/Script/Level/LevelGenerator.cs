using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
using Patterns;
using UnityEngine.Tilemaps;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    int minDepth,maxDepth,maxHorizontalDepth;
    [SerializeField]
    GameObject spawnPrefab, rootPrefab, nodePrefab, linePrefab;
    [SerializeField]
    RoomData[] spawnRoomData, rootRoomData, spineRoomData,normalRoomData;

    [SerializeField]
    Tilemap tileMap,obstacleMap;

    int currMaxDepth;

    LevelGraph graph;
    LevelNode rootNode;
    LevelNode spawnNode;

    private void Start()
    {
        EventManager.Instance.AddListener(EventName.MAP_NODE_CLICKED, Initialize);
        
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventName.MAP_NODE_CLICKED, Initialize);
    }

    void Initialize()
    {
        obstacleMap.ClearAllTiles();
        tileMap.ClearAllTiles();
        graph = new();

        CreateStart();
        CreateSpine();
        CreateBranch();
        AssignRoom();
        AssignPosition();
        GenerateRoom();

        RandomInsertObject();
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
            LevelNode currNode = CreateNode(i, 0, LevelNodeType.SPINE);
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
            node.RoomData = GetRoomData(node.Type);
        }
    }

    RoomData GetRoomData(LevelNodeType type)
    {
        int randIndex;
        switch (type)
        {
            case LevelNodeType.ROOT:
                randIndex = Random.Range(0,rootRoomData.Length);
                return rootRoomData[randIndex];
            case LevelNodeType.SPAWN:
                randIndex = Random.Range(0, spawnRoomData.Length);
                return spawnRoomData[randIndex];
            case LevelNodeType.SPINE:
                randIndex = Random.Range(0, spineRoomData.Length);
                return spineRoomData[randIndex];
            case LevelNodeType.NORMAL:
                randIndex = Random.Range(0, normalRoomData.Length);
                return normalRoomData[randIndex];
            default:
                randIndex = Random.Range(0, normalRoomData.Length);
                return normalRoomData[randIndex];
        }
    }

    void AssignPosition()
    {
        List<LevelNode> spineNodes = graph.GetSpine();

        foreach (LevelNode node in spineNodes)
        {
            int y = CalculateVerticalDistanceFromRoot(node);
            Vector3Int pos = new(0, y, 0);
            node.Position = pos;
            List<LevelNode> leftBranch = graph.GetLeftBranch(node);
            List<LevelNode> rightBranch = graph.GetRightBranch(node);

            foreach (LevelNode curr in leftBranch)
            {
                int x = 0;
                foreach (LevelNode dist in leftBranch)
                {
                    if (dist.DistanceFromSpine < curr.DistanceFromSpine)
                    {
                        x += dist.RoomData.Width - 2;
                    }
                }
                x +=  curr.RoomData.Center.x + node.RoomData.Center.x;
                curr.Position = new(-x + 5, pos.y, 0);
            }

            foreach (LevelNode curr in rightBranch)
            {
                int x = 0;
                foreach (LevelNode dist in rightBranch)
                {
                    if (dist.DistanceFromSpine < curr.DistanceFromSpine)
                    {
                        x += dist.RoomData.Width - 1;
                    }
                }
                x += curr.RoomData.Center.x;
                curr.Position = new(x, pos.y, 0);
                
            }
        }
    }

    int CalculateVerticalDistanceFromRoot(LevelNode node)
    {
        if (node.Depth == -1)
        {
            return 0;
        }

        int distFromPrev = 0;

        foreach (LevelNode prevNode in node.ConnectedNodesPrevDepth)
        {
            if(prevNode.Depth == -1)
            {
                distFromPrev -= prevNode.RoomData.Center.y - 1;
            }
            distFromPrev += CalculateVerticalDistanceFromRoot(prevNode);
            
        }


        return distFromPrev + node.RoomData.Height;       
    }

    void GenerateRoom()
    {
        foreach (var node in graph.NodeList)
        {
            RoomGenerator.GenerateRoom(tileMap,node.RoomData,node.Position);
        }
    }

    List<Vector3Int> GetAvailableObstacleSpawnPosition()
    {
        List<Vector3Int> positionList = new();
        tileMap.CompressBounds();
        BoundsInt bound = tileMap.cellBounds;
        for (int x = bound.xMin; x < bound.xMax; x++)
        {
            for (int y = bound.yMin; y < bound.yMax; y++)
            {
                Vector3Int pos = new(x, y, 0);
                
                if(tileMap.GetTile(pos) != null)
                {
                    string name = tileMap.GetSprite(pos).name;

                    if (name.Contains("Floor"))
                    {
                        Debug.Log("FLOOR FOUND");
                        positionList.Add(pos);
                    }
                }
            }
        }

        return positionList;
    }
    public TileBase testTile;
    [Range(0, 5)]
    public float scale;
    [Range(0, 1)]
    public float per;
    void RandomInsertObject()
    {
        float randVal = Random.Range(0,10);
        foreach (var pos in GetAvailableObstacleSpawnPosition())
        {
            float val = Mathf.PerlinNoise(pos.x * (scale +randVal),pos.y* (scale + randVal));
            if (val > per)
            {
                obstacleMap.SetTile(pos,testTile);
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Initialize();
        }
    }

    #region DEBUGGING
    void Debugging()
    {
        //for visualization
        AssignPosition();
        DisplayMap();
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
