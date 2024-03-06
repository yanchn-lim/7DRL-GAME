using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    int minDepth,maxDepth,maxHorizontalDepth;
    [SerializeField]
    GameObject spawnPrefab, rootPrefab, nodePrefab, linePrefab;

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
        Debugging();
        CreateBranch();
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
            graph.AddEdge(prevNode, currNode);
            prevNode = currNode;
        }
    }

    void CreateBranch()
    {
        int halfDepth = Mathf.FloorToInt(currMaxDepth/2);

        foreach (var node in graph.NodeList)
        {
            if (node.Depth == -1)
                continue;

            uint weight = (uint)node.Depth - (uint)halfDepth;
            Debug.Log(weight);
            
        }
        
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
            //DisplayNode(node, nodePrefab);

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
