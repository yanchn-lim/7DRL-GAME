using System.Collections.Generic;
using UnityEngine;

namespace DataStructure
{
    //The node class where all the information of the node is declared
    public class Node
    {
        public int Id { get; set; }
        public int Depth { get; set; }
        public int IndexInDepth { get; set; }
        public Vector3 Position { get; set; }
        public NodeEncounter EncounterType { get; set; }
        public bool IsAccesible { get; set; }
        public List<Node> AdjacencyList { get; set; }
        public List<Node> ConnectedNodesPrevDepth { get; set; }
        public Node()
        {
            AdjacencyList = new();
            ConnectedNodesPrevDepth = new();
        }

        public Node(int id, int depth)
        {
            Id = id;
            Depth = depth;
            AdjacencyList = new();
            ConnectedNodesPrevDepth = new();

        }
    }

    public enum NodeEncounter
    {
        INFESTED,
        ABANDONED
    }
}