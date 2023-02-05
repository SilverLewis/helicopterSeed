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
    protected Dictionary<Vector3Int?,GridObject> ObjectGrid= new Dictionary<Vector3Int?, GridObject>();
    protected Dictionary<GridObject, Vector3Int?> InverseGrid = new Dictionary<GridObject, Vector3Int?>();
    int hardFloor = -1;
    public GridObject floorReference;
    // Start is called before the first frame update
    void Awake()
    {
        if (singleton)
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GridObject GetObjectAtPosition(Vector3Int? position,bool debug=true)
    {
        GridObject toReturn;
        
        if (ObjectGrid.ContainsKey(position))
        {
            toReturn= ObjectGrid[position];
        }
        else
        {
            if (position.Value.y <= floorReference.transform.position.y)
            {
                if (debug)
                {
                    print("hit rock bottom");
                }
                return floorReference;
            }
            if (debug)
            {
                print("no object at position");
            }
            toReturn = null;
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
