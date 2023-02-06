using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected CollisionGrid gridRef;
    bool referToSingletonGrid=true;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    public virtual void Initialize()
    {
        if (CollisionGrid.instance && referToSingletonGrid)
        {
            gridRef = CollisionGrid.instance;
            //add yourself
            AddToGrid();
        }
    }
    protected virtual void AddToGrid(bool snap=true,bool debug=true)
    {
        if (gridRef.GetPositionOfObject(this,false) == null)
        {
            Vector3Int roundedPos = Vector3Int.RoundToInt(transform.position);
            if (snap) {
                transform.position = roundedPos;
            }
            gridRef.AddObject(this,new Vector3Int?(roundedPos));
        }
        else if(debug)
        {
            //print("tried to add self to grid, but ur already in there.");
        }
    }
    protected virtual void RemoveFromGrid()
    {
        if (gridRef)
        {
            if (gridRef.GetPositionOfObject(this) != null)
            {
                gridRef.RemoveObject(this, false);
            }
        }
    }
    private void OnDestroy()
    {
        RemoveFromGrid();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
