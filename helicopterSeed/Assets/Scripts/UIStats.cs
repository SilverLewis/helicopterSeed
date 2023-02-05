using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText, tallestTreeText, treeCountText, cycleCountText, coinsCollected;
    [SerializeField] TreeManager treeManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = coin.coinCount.ToString()+ " Growth Coins";


    }

    public void UpdateEndText() {
        //UI text
        treeCountText.text = treeManager.getTreeCount().ToString();
        cycleCountText.text = treeManager.getCycleCount().ToString();
        tallestTreeText.text = (1.3 * treeManager.getHieght()).ToString("n2") + "m";
        coinsCollected.text = coin.coinCount.ToString() + " Growth Coins";
    }
}
