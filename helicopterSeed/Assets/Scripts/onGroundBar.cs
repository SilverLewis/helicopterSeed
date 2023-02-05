using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onGroundBar : MonoBehaviour
{
    [SerializeField] RectTransform movingBar;
    [SerializeField] float MaxTimeToDeath = 3;
    [SerializeField]  float TimeToDeath = 3;
    [SerializeField] float recoveryMultiplier = 2;

    [SerializeField] float fadeOutDelay = 1f;
    [SerializeField] float fadeOutCounter;
    [SerializeField] float alphaStep = .01f;

    float maxWidth;

    bool fadingIn = false;
    bool fadingOut = false;

    public float rate;
    
    // Start is called before the first frame update
    void Start()
    {
        fadeOutCounter = 0;
        maxWidth = movingBar.sizeDelta.x;
        TimeToDeath = MaxTimeToDeath;
    }

    private void Update()
    {
        DeathPercent(TimeToDeath/MaxTimeToDeath);

        if (MaxTimeToDeath == TimeToDeath)
        {
            print("full health");

            fadeOutCounter += Time.deltaTime;
            print((fadeOutCounter > fadeOutDelay) + ":" + (!fadingOut) + ":" + (transform.GetChild(0).GetComponent<Image>().color.a > 0));
            if (fadeOutCounter > fadeOutDelay && !fadingOut && transform.GetChild(0).GetComponent<Image>().color.a > 0)
            {
                print("fading out");
                StartCoroutine(FadeOut());
            }
        }
        else {
            fadeOutCounter = 0;
        }
    }

    public void Grounded() {
        TimeToDeath = Mathf.Max(0, TimeToDeath- Time.deltaTime);
        if (transform.GetChild(0).GetComponent<Image>().color.a < 1 && !fadingIn )
        {
            StartCoroutine(FadeIn());
        }
    }

    public void AirBorn()
    {
        TimeToDeath = Mathf.Min(MaxTimeToDeath, TimeToDeath + recoveryMultiplier * Time.deltaTime);
    }

    private void DeathPercent(float percent) {
        movingBar.sizeDelta =new Vector2( maxWidth * percent, movingBar.sizeDelta.y);
    }

    IEnumerator FadeOut()
    {
        fadingOut = true;
        float startingAlpha = transform.GetChild(0).GetComponent<Image>().color.a;
        for (float alpha = startingAlpha; alpha >= 0; alpha -= alphaStep)
        {
            print("here!" + alpha);
            ChangeChildrensAlpha(alpha);
            yield return new WaitForSeconds(alphaStep);
        }
        ChangeChildrensAlpha(0);
        fadingOut = false;
    }

    IEnumerator FadeIn()
    {
        fadingIn = true;
        float startingAlpha = transform.GetChild(0).GetComponent<Image>().color.a; 
        for (float alpha = startingAlpha; alpha < 1; alpha += alphaStep)
        {
            ChangeChildrensAlpha(alpha);
            yield return new WaitForSeconds(alphaStep);
        }
        ChangeChildrensAlpha(1);
        fadingIn = false;
    }

    void ChangeChildrensAlpha(float alpha) {
        for (int i = 0; i < transform.childCount; i++)
        {
            Color c = transform.GetChild(i).GetComponent<Image>().color;
            c.a = alpha;
            transform.GetChild(i).GetComponent<Image>().color = c;
        }
    }
}
