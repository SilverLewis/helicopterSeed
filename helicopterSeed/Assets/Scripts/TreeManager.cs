using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    List<TreeGrid> trees = new List<TreeGrid>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<TreeGrid>()) {
                trees.Add(transform.GetChild(i).GetComponent<TreeGrid>());
                print("here");
            }
        }
    }

    Vector3 GetSpawnPoint() {
        Vector3 highest = new Vector3();
        for (int i =0;i<trees.Count;i++)
        {
            if (trees[i].highestPosition.y > highest.y) {
                highest = trees[i].highestPosition;
            }
        }
        return highest;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.U)) {
            print(GetSpawnPoint());
        }
    }
}
