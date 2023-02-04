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
    List<Transform> currentlyViableGrowPoints;
    public PieceTypes straight, stub, corner, branch2, branch3, branch4, branch5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
