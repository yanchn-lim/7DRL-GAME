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

    [Header("OtherMap")]
    [SerializeField]
    RoomData abadonedRoomData;
    [SerializeField]
    RoomData[] bossRoomData;

    [Header("TileMap + TileBase")]
    [SerializeField]
    Tilemap tileMap,obstacleMap,fogMap;

    [SerializeField]
    TileBase testTile, upgradeTile, fogTile,doorTile;

    int currMaxDepth;

    LevelGraph graph;
    LevelNode rootNode;
    LevelNode spawnNode;

    private void Start()
    {
        EventManager.Instance.AddListener<NodeEncounter>(EventName.MAP_NODE_CLICKED, InitializeBasedOffType);
        
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<NodeEncounter>(EventName.MAP_NODE_CLICKED, InitializeBasedOffType);
    }

    void InitializeBasedOffType(NodeEncounter type)
    {
        switch (type)
        {
            case NodeEncounter.INFESTED:
                InitializeNormalMap();
                break;
            case NodeEncounter.ABANDONED:
                InitializeAbandonedMap();
                break;
            case NodeEncounter.BOSS:
                InitializeBossMap();
                break;
            default:
                InitializeNormalMap();
                break;
        }
    }

    void InitializeNormalMap()
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

        GetAvailableObstacleSpawnPosition();
        RandomInsertObject();
        InsertDoor();
        SpawnFog();
        //Debugging();
    }

    void InitializeAbandonedMap()
    {
        obstacleMap.ClearAllTiles();
        tileMap.ClearAllTiles();
        RoomGenerator.GenerateRoom(tileMap, abadonedRoomData, Vector3Int.zero);
    }

    void InitializeBossMap()
    {
        int index = Random.Range(0, bossRoomData.Length);
        obstacleMap.ClearAllTiles();
        tileMap.ClearAllTiles();
        RoomData data = bossRoomData[index];
        RoomGenerator.GenerateRoom(tileMap,data,Vector3Int.zero);
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

    #region Generating Rooms
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
    #endregion

    void SpawnFog()
    {
        tileMap.CompressBounds();
        BoundsInt bound = tileMap.cellBounds;
        for (int x = bound.xMin; x < bound.xMax; x++)
        {
            for (int y = bound.yMin; y < bound.yMax; y++)
            {
                Vector3Int pos = new(x, y, 0);
                fogMap.SetTile(pos,fogTile);               
            }
        }
    }

    List<Vector3Int> availableSpawnPosition;
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
                        positionList.Add(pos);
                    }
                }
            }
        }
        availableSpawnPosition = positionList;
        return availableSpawnPosition;
    }

    [Range(0, 5)]
    public float scale;
    [Range(0, 1)]
    public float per,per2,per3;

    void RandomInsertObject()
    {
        float randVal = Random.Range(0,10);

        foreach (var pos in availableSpawnPosition)
        {
            float val = Mathf.PerlinNoise(pos.x * (scale +randVal),pos.y* (scale + randVal));
            if (val > per)
            {
                obstacleMap.SetTile(pos,testTile);
                continue;
            }
            else if(val > per2 && val < per3)
            {
                obstacleMap.SetTile(pos, upgradeTile);
                continue;
            }
        }
    }

    [SerializeField]
    int doorMinReach;
    [SerializeField]
    int doorMaxReach;
    [SerializeField]
    int doorDistCheck;
    int doorDistHori = 0;
    int doorDistVert = 0;
    void InsertDoor()
    {
        foreach (var pos in availableSpawnPosition)
        {
            bool doorPlacementCheckHori = CheckDoorHorizontal(pos);
            bool doorPlacementCheckVert = CheckDoorVertical(pos);

            if (doorPlacementCheckHori)
            {
                Debug.Log("PLACING DOORS");
                for (int i = 0; i < doorDistHori; i++)
                {
                    obstacleMap.SetTile(pos + Vector3Int.right * i,doorTile);
                }
            }


            if (doorPlacementCheckVert)
            {
                Debug.Log("PLACING DOORS");
                for (int i = 0; i < doorDistVert; i++)
                {
                    obstacleMap.SetTile(pos + Vector3Int.up * i, doorTile);
                }
            }
        }

    }

    bool CheckDoorHorizontal(Vector3Int pos)
    {
        if (tileMap.GetSprite(pos + Vector3Int.left).name.Contains("Floor"))
        {
            Debug.Log("NO WALL BEHIND");
            return false;
        }

        for (int x = 1; x < doorMaxReach + 1; x++)
        {
            Vector3Int surroundingPos = new(pos.x + x, pos.y, 0);
            if (tileMap.GetTile(surroundingPos) == null)
                continue;

            string tile = tileMap.GetSprite(surroundingPos).name;

            for (int i = 0; i < doorDistCheck; i++)
            {
                Vector3Int checkPosUp = surroundingPos + Vector3Int.up * i;
                Vector3Int checkPosDown = surroundingPos + Vector3Int.down * i;

                if (obstacleMap.GetTile(checkPosUp) != null)
                {
                    if (obstacleMap.GetSprite(checkPosUp).name.Contains("Door"))
                    {
                        return false;
                    }
                }

                if (obstacleMap.GetTile(checkPosDown) != null)
                {
                    if (obstacleMap.GetSprite(checkPosDown).name.Contains("Door"))
                    {
                        return false;
                    }
                }
            }

            if (tile.Contains("Wall") && x >= doorMinReach)
            {
                doorDistHori = x;
                Debug.Log("found wall");
                return true;
            }

            Debug.Log($"TOO FAR : {x}");
        }
        Debug.Log("TOO SHORT");
        return false;
    }

    bool CheckDoorVertical(Vector3Int pos)
    {
        if (tileMap.GetSprite(pos + Vector3Int.down).name.Contains("Floor"))
        {
            Debug.Log("NO WALL BEHIND");
            return false;
        }

        for (int x = 1; x < doorMaxReach + 1; x++)
        {
            Vector3Int surroundingPos = new(pos.x , pos.y + x, 0);
            if (tileMap.GetTile(surroundingPos) == null)
                continue;

            string tile = tileMap.GetSprite(surroundingPos).name;

            for (int i = 0; i < doorDistCheck; i++)
            {
                Vector3Int checkPosUp = surroundingPos + Vector3Int.left * i;
                Vector3Int checkPosDown = surroundingPos + Vector3Int.right * i;

                if (obstacleMap.GetTile(checkPosUp) != null)
                {
                    if (obstacleMap.GetSprite(checkPosUp).name.Contains("Door"))
                    {
                        return false;
                    }
                }

                if (obstacleMap.GetTile(checkPosDown) != null)
                {
                    if (obstacleMap.GetSprite(checkPosDown).name.Contains("Door"))
                    {
                        return false;
                    }
                }
            }

            if (tile.Contains("Wall") && x >= doorMinReach)
            {
                doorDistVert = x;
                Debug.Log("found wall");
                return true;
            }

            Debug.Log($"TOO FAR : {x}");
        }
        Debug.Log("TOO SHORT");
        return false;
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
