using System.Collections.Generic;
using UnityEngine;

namespace DataStructure
{

    //The node class where all the information of the node is declared
    public class Node
    {
        public int Id { get; set; }
        public int Depth { get; set; }
        public Vector3 Position { get; set; }
        public Encounter EncounterType { get; set; }
        //public RandomEvents RandomEvent { get; set; }
        public bool IsAccesible { get; set; }

        private List<GameObject> enemyList = new();

        public void AddEnemy(GameObject enemy)
        {
            enemyList.Add(enemy);
        }

        public enum Encounter
        {
            ENEMY,
            ELITE,
            REST,
            EVENT,
            BOSS
        }

        public List<GameObject> EnemyList
        {
            get
            {
                return enemyList;
            }
        }
    }
}