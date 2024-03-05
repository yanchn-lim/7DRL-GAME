using System.Collections.Generic;
using UnityEngine;
namespace DataStructure
{
    public abstract class Graph
    {
        //node id, node connected
        public Dictionary<int, List<Node>> AdjacencyList;
        //list of all nodes
        public List<Node> NodeList = new List<Node>();

        public int NodeCount = 0;
        public int MaxDepth;
        public Graph()
        {
            AdjacencyList = new Dictionary<int, List<Node>>();
        }

        public void AddNode(Node node)
        {
            // this method adds the node to the node list, adjacency list and edge list;

            NodeList.Add(node);
            AdjacencyList[node.Id] = new List<Node>();
            NodeCount++;

        }

        public void AddEdge(int sourceId, Node target)
        {
            if (AdjacencyList.ContainsKey(sourceId) && AdjacencyList.ContainsKey(target.Id)) //check if the adjacency list contains the source and the target
            {
                AdjacencyList[sourceId].Add(target);
                AdjacencyList[target.Id].Add(GetNode(sourceId));
            }
        }

        public Node GetNode(int id)
        {
            foreach (var node in NodeList)
            {
                if(node.Id == id)
                {
                    return node;
                }
            }

            return null;
        }

        public void RemoveNode(Node node)
        {
            //removing instances of the removed node from the edge list
            foreach (var item in AdjacencyList)
            {
                List<Node> connectedNodes = item.Value;
                if (connectedNodes.Contains(node))
                {
                    connectedNodes.Remove(node);
                }
            }
            //removing the node from the adjacency list
            AdjacencyList.Remove(node.Id);
            NodeList.Remove(node);

            //NO IMPLEMENTATION OF REMOVING EDGES FROM EDGELIST YET

        }

        public List<Node> GetConnected(int id)
        {
            if (AdjacencyList.ContainsKey(id))//searches the list and returns a list of nodes connected
            {
                return AdjacencyList[id];
            }

            return new List<Node>();
        }

        public List<Node> GetConnectedNextDepth(int id)
        {
            List<Node> nextDepth = new();
            if (AdjacencyList.ContainsKey(id))//searches the list and returns a list of nodes connected
            {
                foreach (var node in AdjacencyList[id])
                {
                    if(node.Depth == GetNode(id).Depth + 1)
                    {
                        nextDepth.Add(node);
                    }
                }
                return nextDepth;
            }

            return null;
        }

        public List<Node> GetNodesInDepth(int depth)
        {
            List<Node> NodesInDepth = new List<Node>();

            foreach (Node node in NodeList) //looping the list of nodes
            {
                if (node.Depth == depth)//checking depth
                {
                    NodesInDepth.Add(node);
                }
            }

            return NodesInDepth;
        }

        public bool IsConnectedGraph()
        {
            System.Diagnostics.Stopwatch time = new();
            time.Start();
            List<Node> visited = new();
            
            List<Node> nodeFirstDepth = GetNodesInDepth(0);
            int randIndex = Random.Range(0, nodeFirstDepth.Count);
            Search(nodeFirstDepth[randIndex], visited);

            Debug.Log($"VISITED : {visited.Count} \nADJACENTLIST : {AdjacencyList.Count}");
            Debug.Log($"TIME ELAPSED : {time.ElapsedMilliseconds}");
            return visited.Count == AdjacencyList.Count;
        }

        public void Search(Node node, List<Node> visited)
        {
            if (!visited.Contains(node))
            {
                visited.Add(node);

                if(AdjacencyList.TryGetValue(node.Id,out List<Node> neighbour))
                {
                    foreach (var n in neighbour)
                    {
                        Search(n, visited);
                    }
                }
            }
        }
    }

}

