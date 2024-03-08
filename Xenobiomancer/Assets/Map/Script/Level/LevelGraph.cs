using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
using System.Linq;

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

    public List<LevelNode> GetLeftBranch(LevelNode spine)
    {
        if(spine.HorizontalDepth != 0)
        {
            Debug.Log("not spine");
            return null;
        }

        List<LevelNode> leftBranch = NodeList.Where(item => item.HorizontalDepth < 0 && item.Depth == spine.Depth).ToList();

        return leftBranch;
    }

    public List<LevelNode> GetRightBranch(LevelNode spine)
    {
        if (spine.HorizontalDepth != 0)
        {
            Debug.Log("not spine");
            return null;
        }

        List<LevelNode> rightBranch = NodeList.Where(item => item.HorizontalDepth > 0 && item.Depth == spine.Depth).ToList();

        return rightBranch;
    }

    public List<LevelNode> GetBranchInDepth(LevelNode spine)
    {
        if (spine.HorizontalDepth != 0)
        {
            Debug.Log("not spine");
            return null;
        }

        List<LevelNode> branch = NodeList.Where(item => item.Depth == spine.Depth && spine != item).ToList();

        return branch;
    }
}
