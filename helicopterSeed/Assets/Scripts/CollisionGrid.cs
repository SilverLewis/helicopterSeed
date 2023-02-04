using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// grid for generalized dictionary based grid collision. tracks all the gridObjects in the scene
/// </summary>

public class CollisionGrid : MonoBehaviour
{
    public bool singleton;
    public static CollisionGrid instance;
    Dictionary<Vector3Int?,GridObject> ObjectGrid= new Dictionary<Vector3Int?, GridObject>();
    Dictionary<GridObject, Vector3Int?> InverseGrid = new Dictionary<GridObject, Vector3Int?>();
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null && singleton)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GridObject GetObjectAtPosition(Vector3Int? position)
    {
        GridObject toReturn = ObjectGrid[position];
        if (toReturn == null)
        {
            print("no object at position");
            
        }
        return toReturn;
    }
    
    public Vector3Int? GetPositionOfObject(GridObject objectToCheck,bool debug=true)
    {
        bool realPosition = InverseGrid.ContainsKey(objectToCheck);
        if (!realPosition)
        {
            if (debug)
            {
                print("object not in grid");
            }
            return null;
        }
        else
        {
            return InverseGrid[objectToCheck];
        }
    }

    public bool AddObject(GridObject objectToAdd,Vector3Int? position,bool overwrite=false)
    {
        bool preExistingObject = ObjectGrid.ContainsKey(position);
        if (preExistingObject)
        {
            if (!overwrite)
            {
                print("there's already something there");
                return false;
            }
            else
            {
                //destroy the pre-existing object
                RemoveObject(ObjectGrid[position]);
            }
        }

        ObjectGrid.Add(position, objectToAdd);
        InverseGrid.Add(objectToAdd, position);

        return true;
        
    }

    public bool RemoveObject(GridObject objectToRemove,bool destroy=true)
    {
        Vector3Int? posOfGuy = GetPositionOfObject(objectToRemove);
        if (posOfGuy == null)
        {
            print("cant remove, guy has no position in grid");
            return false;
        }
        else
        {
            ObjectGrid.Remove(posOfGuy);
            InverseGrid.Remove(objectToRemove);
            if (destroy)
            {
                Destroy(objectToRemove.gameObject);
            }
            return true;
        }
    }
}
