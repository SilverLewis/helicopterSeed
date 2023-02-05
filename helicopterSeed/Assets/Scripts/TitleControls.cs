using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleControls : MonoBehaviour
{
    [SerializeField] ScreenFader fader;
    bool loadin = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")){
            Application.Quit();
        }else if ((Input.anyKeyDown || Input.GetButtonDown("Jump")) && !loadin)
        {
            loadin = true;
            fader.FadeImage(1, 0);
            Invoke("DelayedLoad",fader.timeToFade+fader.delay);
        }
    }
    void DelayedLoad()
    {
        SceneManager.LoadScene("THE REAL SCENE");
    }
}
