using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeManager : MonoBehaviour
{
    List<TreeGrid> trees = new List<TreeGrid>();
    [SerializeField] GameObject freshTreePrefab;
    [SerializeField] GameObject player,winText,loseText;
    [SerializeField] onGroundBar barRef;
    [SerializeField] ScreenFader loadUI;
    [SerializeField] float minuimStatTime ;

    [SerializeField] Transform endGameStatsTextHolder;
    [SerializeField] float alphaStep = .01f;

    // Start is called before the first frame update
    void Start()
    {
        //stat text starst at 0 alpha;
        ChangeChildrensAlpha(0);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<TreeGrid>()) {
                trees.Add(transform.GetChild(i).GetComponent<TreeGrid>());
            }
        }
        coin.coinCount = 20;
        StartCoroutine(IterateAllTrees());
        coin.coinCount = 0;
        player.transform.position = GetSpawnPoint();
        player.gameObject.SetActive(true);

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
        yield return new WaitForSeconds(1);//time you get to just look at the win / loss on the ground
        loadUI.FadeImage(1, 0);
        yield return new WaitForSeconds(loadUI.timeToFade+loadUI.delay);
        
        loseText.SetActive(false);
        winText.SetActive(false);

        player.gameObject.SetActive(false);
        Coroutine iter = StartCoroutine(IterateAllTrees());

        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(minuimStatTime);

        yield return iter;

        coin.coinCount = 0;
        player.transform.position = GetSpawnPoint();
        player.gameObject.SetActive(true);
        barRef.ResetShit();
        
        StartCoroutine(FadeOut());
        loadUI.FadeImage(0, 1);

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

    IEnumerator FadeOut()
    {
        float startingAlpha = endGameStatsTextHolder.GetChild(0).GetComponent<TextMeshProUGUI>().color.a;
        for (float alpha = startingAlpha; alpha >= 0; alpha -= alphaStep)
        {
            ChangeChildrensAlpha(alpha);
            yield return new WaitForSeconds(alphaStep);
        }
        ChangeChildrensAlpha(0);
    }

    IEnumerator FadeIn()
    {
        float startingAlpha = endGameStatsTextHolder.GetChild(0).GetComponent<TextMeshProUGUI>().color.a;
        for (float alpha = startingAlpha; alpha < 1; alpha += alphaStep)
        {
            ChangeChildrensAlpha(alpha);
            yield return new WaitForSeconds(alphaStep);
        }
        ChangeChildrensAlpha(1);
    }

    void ChangeChildrensAlpha(float alpha)
    {
        for (int i = 0; i < endGameStatsTextHolder.childCount; i++)
        {
            Color c = endGameStatsTextHolder.GetChild(i).GetComponent<TextMeshProUGUI>().color;
            c.a = alpha;
            endGameStatsTextHolder.GetChild(i).GetComponent<TextMeshProUGUI>().color = c;
        }
    }

}
