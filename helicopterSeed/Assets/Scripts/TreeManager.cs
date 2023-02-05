using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    List<TreeGrid> trees = new List<TreeGrid>();
    [SerializeField] GameObject freshTreePrefab;
    [SerializeField] GameObject player,winText,loseText;
    [SerializeField] onGroundBar barRef;
    [SerializeField] ScreenFader loadUI;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<TreeGrid>()) {
                trees.Add(transform.GetChild(i).GetComponent<TreeGrid>());
            }
        }

        player.transform.position = GetSpawnPoint();
        player.gameObject.SetActive(true);

        StartCoroutine(IterateAllTrees());
        loadUI.FadeImage(0, 1);
    }

    Vector3 GetSpawnPoint() {
        Vector3 highest = new Vector3();
        for (int i =0;i<trees.Count;i++)
        {
            if (trees[i].highestPosition.y > highest.y) {
                highest = trees[i].highestPosition;
            }
        }
        print(highest);
        return highest+Vector3.up*20;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.U)) {
            print(GetSpawnPoint());
        }
    }
    public void OnPlayerLanded(bool landedOnSoil)
    {
        if (landedOnSoil)
        {
            winText.SetActive(true);
            Vector3Int treeSpawn = Vector3Int.RoundToInt(player.transform.position)+Vector3Int.down;
            player.gameObject.SetActive(false);
            print(treeSpawn + " new tree here!");
            GameObject newTree = Instantiate(freshTreePrefab, treeSpawn, Quaternion.identity, transform);
            if (newTree.GetComponent<TreeGrid>())
            {
                trees.Add(newTree.GetComponent<TreeGrid>());
            }
        }
        else
        {
            loseText.SetActive(true);
        }
        StartCoroutine(RespawnProcess());

    }
    IEnumerator RespawnProcess()
    {
        yield return new WaitForSeconds(2);//time you get to just look at the win / loss on the ground
        loadUI.FadeImage(1, 0);
        yield return new WaitForSeconds(loadUI.timeToFade+loadUI.delay);
        player.gameObject.SetActive(false);
        yield return StartCoroutine(IterateAllTrees());
        player.transform.position = GetSpawnPoint();
        player.gameObject.SetActive(true);
        barRef.ResetShit();
        loadUI.FadeImage(0, 1);
        loseText.SetActive(false);
        winText.SetActive(false);
        player.GetComponentInChildren<SeedMovement>().enabled = true;
        //the timer would have to not start till the fader ends

    }
    /// <summary>
    /// ill do this as an ienumerator so theres like, a chance at a loading bar
    /// </summary>
    /// <returns></returns>
    IEnumerator IterateAllTrees()
    {
        foreach(TreeGrid eachTree in trees)
        {
            eachTree.YearlyIteration();
            yield return null;
            //this is where a loading bar would update
        }
    }
    
}
