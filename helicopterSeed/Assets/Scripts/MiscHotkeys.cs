using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiscHotkeys : MonoBehaviour
{
    [SerializeField] ScreenFader[] faders;
    bool loadin = false;
    [SerializeField] AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")) && !loadin)
        {

            loadin = true;
            foreach(ScreenFader fader in faders)
            {
                fader.FadeImage(1, 0);
            }
            Invoke("DelayedLoad", faders[0].timeToFade + faders[0].delay);
        }else if (Input.GetKeyDown(KeyCode.R) && !loadin)
        {

            loadin = true;
            foreach (ScreenFader fader in faders)
            {
                fader.FadeImage(1, 0);
            }
            Invoke("DelayedReload", faders[0].timeToFade + faders[0].delay);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            music.mute = !music.mute;
        }
        
    }

    
    void DelayedLoad()
    {
        SceneManager.LoadScene("Title");
    }

    void DelayedReload()
    {
        SceneManager.LoadScene("THE REAL SCENE");

    }
}
