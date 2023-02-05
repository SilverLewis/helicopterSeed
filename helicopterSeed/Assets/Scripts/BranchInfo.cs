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
    public List<Transform> freeGrowPoints= new List<Transform>();
    public float weight=500; //how much to prioritize growing from this branch
    float upWeight=700, downWeight=-300, sideWeight=-100,heightWeight=400;
    // Start is called before the first frame update
    public override void Initialize()
    {
        if (treeRef == null)
        {
            treeRef = transform.parent.GetComponent<TreeGrid>();
            if (treeRef == null)
            {
                print("this piece isnt in a tree!");
            }
        }
        base.Initialize();
    }
    protected override void AddToGrid(bool snap = true,bool debug=true)
    {
        base.AddToGrid(snap,true);
        if (treeRef)
        {
            if (treeRef.GetPositionOfObject(this) == null)
            {
                print("added object to tree");
                treeRef.AddObject(this, new Vector3Int?(Vector3Int.RoundToInt(transform.position)));
                treeRef.currentlyViableBranches.Add(this);//assume a new one is viable, the quick check will determine if it is or not
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

    public void DetermineFreeGrowPoints()
    {
        weight = 500;
        foreach(Transform branchPoint in branchingPoints)
        {
            Vector3Int positionAlongGrowPoint =Vector3Int.RoundToInt( transform.position + branchPoint.up*2);
            //Debug.DrawLine(transform.position, positionAlongGrowPoint,Color.red,1000);
            if (gridRef.GetObjectAtPosition(new Vector3Int?(positionAlongGrowPoint),false) == null)
            {
                //if theres nothing in the way its a free grow point
                if (!freeGrowPoints.Contains(branchPoint))
                {
                    freeGrowPoints.Add(branchPoint);
                    print("found a free point");
                }
                float upNess = Mathf.Round(branchPoint.up.y);
                if (upNess >0)
                {
                    weight += upWeight;
                }
                else if (upNess == 0 )
                {
                    weight += sideWeight;
                }
                else if (upNess < 0)
                {
                    weight += downWeight;
                }
            }
            else
            {
                //prints hwats in tha way
                //print(gridRef.GetObjectAtPosition(new Vector3Int?(positionAlongGrowPoint), false));
                //if theres something in the way its not free
                if (freeGrowPoints.Contains(branchPoint))
                {
                    freeGrowPoints.Remove(branchPoint);
                    print("removed a blocked point"); 
                    
                }
            }
        }
        weight += transform.position.y * heightWeight;
    }

    /// <summary>
    /// returns a random grow point to spawn from. made this its own function if we want any more logic to this later (like caring about shade or some shit?)
    /// </summary>
    /// <returns></returns>
    public Transform RandomGrowPoint()
    {
        Transform chosenGrowPoint = freeGrowPoints[Random.Range(0, freeGrowPoints.Count)];
        return chosenGrowPoint;
    }
}
