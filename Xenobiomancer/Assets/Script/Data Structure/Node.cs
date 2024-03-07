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
        public LevelNodeType Type { get; set; }
        public int HorizontalDepth { get; set; }
        public int DistanceFromSpine { get { return Mathf.Abs(HorizontalDepth); } }
        public List<EntranceDirection> Entrances { get; }
        public RoomData RoomData { get; set; }
        public Vector3Int Position { get; set; }

        #region FOR DEBUGGING
        public Color DebugColor { get; set; }
        float weight = -1;
        public float DebugWeight { get { return weight; } set { weight = value; } }
        #endregion

        public LevelNode()
        {
            Entrances = new();
        }
    }

    public enum EntranceDirection
    {
        NORTH,
        SOUTH,
        EAST,
        WEST
    }

    public enum LevelNodeType
    {
        ROOT,
        SPAWN,
        SPINE,
        NORMAL
    }

    public enum NodeEncounter
    {
        INFESTED,
        ABANDONED,
        BOSS
    }
}