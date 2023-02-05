using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanLines : MonoBehaviour {
	[SerializeField] Material myMat;
    [SerializeField] string textureToScroll="_MainTex";
    [SerializeField] float xSpeed, ySpeed;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        myMat.SetTextureOffset(textureToScroll, new Vector2(myMat.GetTextureOffset(textureToScroll).x + xSpeed * Time.deltaTime, myMat.mainTextureOffset.y + ySpeed * Time.deltaTime));
	}
}
