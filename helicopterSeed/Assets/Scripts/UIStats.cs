using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText, tallestTreeText, treeCountText, cylceCountText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = coin.coinCount.ToString()+ "Growth Coins";
    }
}
