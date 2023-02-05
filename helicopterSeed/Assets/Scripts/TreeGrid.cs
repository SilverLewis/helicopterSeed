using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PieceTypes
{
    [Tooltip("the heigher the number, the more likely it is to pick this type")]
    public float weight;
    public GameObject[] prefabs;
    
}
public class TreeGrid : CollisionGrid
{
    [SerializeField] int minGrowthTokens, maxGrowthTokens;
    List<BranchInfo> currentlyViableBranches = new List<BranchInfo>();
    public PieceTypes straight, stub, corner, branch2, branch3, branch4, branch5;
    PieceTypes[] allPieceTypes;
    float[] legalRotations = new float[] { 0, 90, 180, 270 };
    float weightRecencyBias=30;
    // Start is called before the first frame update
    void Start()
    {
        allPieceTypes = new PieceTypes[] { straight, stub, corner, branch2, branch3, branch4, branch5 };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            YearlyIteration();
        }
    }
    void YearlyIteration()
    {
        int tokensToSpend = Random.Range(minGrowthTokens, maxGrowthTokens);// this can be replaced with a method if we ever make it more variable than that
        int tokensSpent = 0;
        for (tokensSpent = 0; tokensSpent < tokensToSpend; tokensSpent++)
        {
            BranchInfo placeToSpawnFrom = GetViableSpot(); //this updates knowledge of which spawns are gonna be viable and which are blocked
            if (placeToSpawnFrom == null)
            {
                print("damn no spawns available! ending iteration!");
                break;
            }

            //choose a prefab
            GameObject newBranch = PickBranchType();

            //add it to the tree
            PlaceNewPiece(placeToSpawnFrom, newBranch);
        }


        DetermineCurrentlyViableBranches(); //just doing this again so the weights end up accurate
        if (tokensSpent != tokensToSpend)
        {
            print("had " + (tokensToSpend - tokensSpent) + " tokens remaining");
        }
    }

    void PlaceNewPiece(BranchInfo placeToSpawnFrom, GameObject prefab)
    {
        Transform spawnConnectionPoint = placeToSpawnFrom.RandomGrowPoint();
        Vector3 spawnPosition = placeToSpawnFrom.transform.position + spawnConnectionPoint.up*2;
        GameObject newPiece = Instantiate(prefab, spawnPosition, spawnConnectionPoint.rotation, transform);
        //randomly rotate along the axis it's growing off of
        newPiece.transform.Rotate(spawnConnectionPoint.up, legalRotations[Random.Range(0, legalRotations.Length)],Space.Self);
        newPiece.transform.up = spawnConnectionPoint.up;
        newPiece.GetComponent<BranchInfo>().Initialize();//so you dont have to wait for it's start, adds it to the gridses
    }

    GameObject PickBranchType()
    {

        //if you wanna fuck with the weights in runtime this would be when
        float weightSum = straight.weight+corner.weight+stub.weight+branch2.weight+branch3.weight+branch4.weight+branch5.weight; 

        float rando = Random.Range(0, weightSum);
        PieceTypes typeChosen=straight;
        float total = 0;
        int typeIndex = 0;
        for (typeIndex = 0; typeIndex < allPieceTypes.Length; typeIndex++)
        {
            total += allPieceTypes[typeIndex].weight;
            if (total >= rando)
            {
                typeChosen = allPieceTypes[typeIndex];
                break;
            }
        }
        foreach(PieceTypes eachType in allPieceTypes)
        {
            if (typeChosen != eachType)
            {
                typeChosen.weight += weightRecencyBias;
            }
            else
            {
                typeChosen.weight -= weightRecencyBias;
            }
        }
        return typeChosen.prefabs[Random.Range(0, typeChosen.prefabs.Length)];
    }
    BranchInfo GetViableSpot()
    {
        DetermineCurrentlyViableBranches();
        if (currentlyViableBranches.Count > 0)
        {
            //get weighted branch, prioritized by height and upwards ness 
            float weightSum = 0;
            foreach(BranchInfo eachBranch in currentlyViableBranches)
            {
                weightSum += eachBranch.weight;
            }
            float rando = Random.Range(0, weightSum);
            float total = 0;
            int index = 0;
            BranchInfo chosenBranch=currentlyViableBranches[0];
            for (index = 0; index < currentlyViableBranches.Count; index++)
            {
                total += currentlyViableBranches[index].weight;
                if (total >= rando)
                {
                    chosenBranch = currentlyViableBranches[index];
                    break;
                }
            }

            //return currentlyViableBranches[Random.Range(0, currentlyViableBranches.Count)];
            return chosenBranch;
        }
        else
        {
            //0 branches available on the whole tree!
            return null;
        }
    }

    void DetermineCurrentlyViableBranches()
    {
        //print(ObjectGrid.Values.Count);
        foreach(BranchInfo eachBranchPiece in ObjectGrid.Values)
        {
            eachBranchPiece.DetermineFreeGrowPoints();
            if (eachBranchPiece.freeGrowPoints.Count > 0)
            {
                //if so then its viable to branch out from
                if (!currentlyViableBranches.Contains(eachBranchPiece))
                {
                    currentlyViableBranches.Add(eachBranchPiece);
                }
            }
            else
            {
                if (currentlyViableBranches.Contains(eachBranchPiece))
                {
                    currentlyViableBranches.Remove(eachBranchPiece);
                }
            }
        }
    }
}
