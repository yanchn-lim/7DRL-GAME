using System.Collections.Generic;
using UnityEngine;
namespace DataStructure
{
    public abstract class Graph<T> where T : Node
    {
        //node id, node connected
        public Dictionary<int, List<T>> AdjacencyList;
        //list of all nodes
        public List<T> NodeList = new List<T>();

        public int NodeCount = 0;
        public int MaxDepth;
        public Graph()
        {
            AdjacencyList = new Dictionary<int, List<T>>();
        }

        public void AddNode(T node)
        {
            // this method adds the node to the node list, adjacency list and edge list;

            NodeList.Add(node);
            AdjacencyList[node.Id] = new List<T>();
            NodeCount++;

            if (node.Depth > MaxDepth)
                MaxDepth = node.Depth;

        }

        public void AddEdge(int sourceId, T target)
        {
            if (AdjacencyList.ContainsKey(sourceId) && AdjacencyList.ContainsKey(target.Id)) //check if the adjacency list contains the source and the target
            {
                AdjacencyList[sourceId].Add(target);
                AdjacencyList[target.Id].Add(GetNode(sourceId));
                GetNode(sourceId).AdjacencyList.Add(target);
                target.ConnectedNodesPrevDepth.Add(GetNode(sourceId));
            }
        }

        public void AddEdge(T source, T target)
        {
            if (AdjacencyList.ContainsKey(source.Id) && AdjacencyList.ContainsKey(target.Id)) //check if the adjacency list contains the source and the target
            {
                AdjacencyList[source.Id].Add(target);
                AdjacencyList[target.Id].Add(GetNode(source.Id));
                source.AdjacencyList.Add(target);
                target.ConnectedNodesPrevDepth.Add(source);

            }
        }

        public T GetNode(int id)
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

        public void RemoveNode(T node)
        {
            //removing instances of the removed node from the edge list
            foreach (var item in AdjacencyList)
            {
                List<T> connectedNodes = item.Value;
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

        public List<T> GetConnected(int id)
        {
            if (AdjacencyList.ContainsKey(id))//searches the list and returns a list of nodes connected
            {
                return AdjacencyList[id];
            }

            return new List<T>();
        }

        public List<T> GetConnectedNextDepth(int id)
        {
            List<T> nextDepth = new();
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

        public List<T> GetNodesInDepth(int depth)
        {
            List<T> NodesInDepth = new List<T>();

            foreach (var node in NodeList) //looping the list of nodes
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
            List<T> visited = new();
            
            List<T> nodeFirstDepth = GetNodesInDepth(0);
            int randIndex = Random.Range(0, nodeFirstDepth.Count);
            Search(nodeFirstDepth[randIndex], visited);

            Debug.Log($"VISITED : {visited.Count} \nADJACENTLIST : {AdjacencyList.Count}");
            Debug.Log($"TIME ELAPSED : {time.ElapsedMilliseconds}");
            return visited.Count == AdjacencyList.Count;
        }

        public void Search(T node, List<T> visited)
        {
            if (!visited.Contains(node))
            {
                visited.Add(node);

                if(AdjacencyList.TryGetValue(node.Id,out List<T> neighbour))
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

