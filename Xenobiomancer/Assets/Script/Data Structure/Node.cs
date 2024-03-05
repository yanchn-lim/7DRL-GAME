using System.Collections.Generic;
using UnityEngine;

namespace DataStructure
{
    //The node class where all the information of the node is declared
    public abstract class Node
    {
        public int Id { get; set; }
        public int Depth { get; set; }
        public int IndexInDepth { get; set; }
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

    public class MapNode : Node 
    {
        public NodeEncounter EncounterType { get; set; }
        public Vector3 Position { get; set; }
        public bool IsAccesible { get; set; }
        public NodeObject Object { get; set; }
        
    }


    public class LevelNode : Node
    {

    }

    public enum NodeEncounter
    {
        INFESTED,
        ABANDONED,
        BOSS
    }

    public enum LevelRoom 
    {
        SPAWN,
        ROOM_1,
        ROOM_2,
        ROOM_3,
        ROOM_4,
        ROOM_5
    }
}