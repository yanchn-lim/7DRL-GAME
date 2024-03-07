using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
public class LevelGraph : Graph<LevelNode>
{
    public List<LevelNode> GetSpine()
    {
        List<LevelNode> spineNodes = new();
        foreach (var node in NodeList)
        {
            if (node.HorizontalDepth != 0)
                continue;

            spineNodes.Add(node);
        }

        return spineNodes;
    }
}
