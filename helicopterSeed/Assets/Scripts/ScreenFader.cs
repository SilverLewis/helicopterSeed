using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] Image fader;
    public float timeToFade,delay;
    [SerializeField] bool fadeInOnStart,fadeOutOnStart,fadeMaterial;
    [SerializeField] TextMeshProUGUI debug;
    AudioSource audioGuy;
    public bool isFading;

    // Start is called before the first frame update
    void Start()
    {
        
        audioGuy = GetComponent<AudioSource>();
        if (fadeInOnStart)
        {
            if (fadeMaterial)
            {
                StartCoroutine(FadeMaterial(0, 1));
            }
            else { 
                StartCoroutine(Fade(0, 1));
            }
        }else if (fadeOutOnStart) //literally just for the title
        {
            if (fadeMaterial)
            {
                StartCoroutine(FadeMaterial(.7f, 0));
            }
            else
            {
                StartCoroutine(Fade(.7f, 0));
            }
        }
    }
    public void FadeImage(float targetAlpha,float startAlpha,float newDelay=-1)
    {
        fader.enabled = true;
        if (fadeMaterial)
        {
            StartCoroutine(FadeMaterial(targetAlpha, startAlpha, newDelay));
        }
        else
        {
            StartCoroutine(Fade(targetAlpha, startAlpha, newDelay));
        }
    }
    IEnumerator Fade(float targetAlpha,float startAlpha,float newDelay=-1)
    {
        isFading = true;
        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, startAlpha);
        if (newDelay >= 0)
        {
            yield return new WaitForSeconds(newDelay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }
        if (audioGuy)
        {
            audioGuy.Play();
        }
        //fader.CrossFadeAlpha(targetAlpha, timeToFade, false);
        float i = 0;
        float currentAlpha;
        while (i <= timeToFade)
        {
            i += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, i/timeToFade);

            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, currentAlpha);
            yield return null;
        }

        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, targetAlpha);
        isFading = false;
    }

    IEnumerator FadeMaterial(float targetAlpha, float startAlpha, float newDelay = -1)
    {
        isFading = true;
        if (debug != null)
        {
            debug.text = "strangth: " + fader.material.GetFloat("_DistortionStrength");
        }
        //fader.material.SetFloat("_DistortionStrength", startAlpha);
        fader.material.SetFloat("_DistortionBlend", 1 - startAlpha);
        if (newDelay >= 0)
        {
            yield return new WaitForSeconds(newDelay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }
        audioGuy.Play();
        //fader.CrossFadeAlpha(targetAlpha, timeToFade, false);
        float i = 0;
        float currentAlpha;
        while (i <= timeToFade)
        {
            i += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, i / timeToFade);
            //fader.material.SetFloat("_DistortionStrength",currentAlpha);
            fader.material.SetFloat("_DistortionBlend",1-currentAlpha);
            if (debug != null)
            {
                debug.text ="strength: " + fader.material.GetFloat("_DistortionStrength");
            }
            yield return null;
        }

        //fader.material.SetFloat("_DistortionStrength", targetAlpha);
        fader.material.SetFloat("_DistortionBlend", 1 - targetAlpha);
        
        isFading = false; 
        if (debug!=null)
        {
            debug.text = "strongth: " + fader.material.GetFloat("_DistortionStrength");
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L)){
        //    fader.enabled = !fader.enabled;
        //}
    }
}
