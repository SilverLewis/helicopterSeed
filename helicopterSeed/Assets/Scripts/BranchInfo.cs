using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// information about each branch piece
/// </summary>
public class BranchInfo : GridObject
{
    [SerializeField] Transform[] branchingPoints, leafPoints;
    //[SerializeField] Vector3[] legalRotations;
    TreeGrid treeRef;
    List<Transform> freeGrowPoints;
    // Start is called before the first frame update
    protected override void Initialize()
    {
        base.Initialize();
        if (treeRef == null)
        {
            treeRef = transform.parent.GetComponent<TreeGrid>();
            if (treeRef == null)
            {
                print("this piece isnt in a tree!");
            }
        }
    }
    protected override void AddToGrid(bool snap = true,bool debug=true)
    {
        base.AddToGrid(snap,true);
        if (treeRef)
        {
            if (treeRef.GetPositionOfObject(this) == null)
            {
                treeRef.AddObject(this, new Vector3Int?(Vector3Int.CeilToInt(transform.position)));
            }
        }
    }
    protected override void RemoveFromGrid()
    {
        base.RemoveFromGrid();
        if (treeRef)
        {
            if (treeRef.GetPositionOfObject(this) != null)
            {
                treeRef.RemoveObject(this, false);
            }
        }
    }
}
