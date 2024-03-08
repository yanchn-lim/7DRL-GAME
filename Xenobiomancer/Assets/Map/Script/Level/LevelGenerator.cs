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
        AssignRoom();
        AssignPosition();
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

            foreach (LevelNode curr in graph.GetLeftBranch(node))
            {
                int x = 0;
                foreach (LevelNode dist in graph.GetLeftBranch(node))
                {
                    if (curr.DistanceFromSpine > dist.DistanceFromSpine)
                        continue;
                    x += dist.RoomData.Width;
                }
                x += node.RoomData.Center.x;
                curr.Position = new(-x, pos.y, 0);
            }

            foreach (LevelNode curr in graph.GetRightBranch(node))
            {
                int x = 0;
                foreach (LevelNode dist in graph.GetRightBranch(node))
                {
                    if (curr.DistanceFromSpine > dist.DistanceFromSpine)
                        continue;
                    x += dist.RoomData.Width;
                }
                x -= node.RoomData.Center.x;
                curr.Position = new(x, pos.y, 0);
            }
        }

        //foreach (LevelNode node in graph.NodeList)
        //{
        //    if (node.HorizontalDepth == 0)
        //        continue;
        //    LevelNode root = spineNodes.First(item => item.Depth == node.Depth);
        //    int y = root.Position.y;
        //    int x = CalculateHorizontalDistanceFromRoot(node);
        //    //int x = 0;
        //    Vector3Int pos = new(x,y, 0);
        //    node.Position = pos;
        //}
    }

    int CalculateHorizontalDistanceFromRoot(LevelNode node)
    {
        if (node.HorizontalDepth == 0)
        {
            return 0;
        }

        int distFromPrev = 0;

        foreach (LevelNode prevNode in node.ConnectedNodesPrevDepth)
        {
            if (prevNode.Depth == node.Depth && prevNode.DistanceFromSpine < node.DistanceFromSpine)
            {
                if (node.DistanceFromSpine == 1 && prevNode.HorizontalDepth == 0)
                {
                    if (node.HorizontalDepth < 0)
                    {
                        distFromPrev -= prevNode.RoomData.Center.x - 7;
                    }
                    else
                    {
                        distFromPrev += prevNode.RoomData.Center.x - 9;
                    }
                    continue;
                }

                distFromPrev += CalculateHorizontalDistanceFromRoot(prevNode);
            }
        }

        if (node.HorizontalDepth < 0)
        {
            return distFromPrev - node.RoomData.Width + 1;
        }
        else
        {
            return distFromPrev + node.RoomData.Width - 1;
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

    void RecursionDebug()
    {
        
    }

    #endregion
}
