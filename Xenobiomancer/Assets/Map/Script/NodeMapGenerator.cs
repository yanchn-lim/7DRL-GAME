using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
using Patterns;
public class NodeMapGenerator : MonoBehaviour
{
    [SerializeField]
    int MaxDepth,MinNodesPerDepth, MaxNodesPerDepth;
    Graph graph;
    
    void InitializeMap()
    {
        graph = new();
        //move to start of game
        RandSeedManager.GenerateSeed();

        //GENERATE NODES

        //GENERATE THE EDGES

        

        //DISPLAY THE NODE + EDGES

    }

    void GenerateNodes()
    {
        for (int depth = 0; depth < MaxDepth; depth++)
        {
            int numNodes = Random.Range(MinNodesPerDepth, MaxNodesPerDepth);
            for (int j = 0; j < numNodes; j++)
            {
                //generate nodes here
                Node node = new();
                node.Id = graph.NodeCount;
                node.Depth = depth;

                if (depth == 0)
                {
                    node.EncounterType = Node.Encounter.INFESTED;
                    node.IsAccesible = true;
                }
                else
                {
                    //randomly select node encounter here
                    node.EncounterType = Node.Encounter.INFESTED;
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

    }
}
