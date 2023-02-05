using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpAudio : MonoBehaviour
{
    [SerializeField] AudioClip wood, rock, grass, water, leaf;

    AudioSource audioRef;
    float defaultVolume,defaultPitch;
    [SerializeField] float pitchVar, volumeVar;
    // Start is called before the first frame update
    void Start()
    {
        audioRef = GetComponent<AudioSource>();
        if (audioRef)
        {
            defaultPitch = audioRef.pitch;
            defaultVolume = audioRef.volume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayRandomized(AudioClip clip)
    {
        if (audioRef)
        {
            audioRef.pitch = Random.Range(defaultPitch - pitchVar, defaultPitch + pitchVar);
            audioRef.PlayOneShot(clip, Random.Range(defaultVolume - volumeVar, defaultVolume + volumeVar));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        string theTag = collision.collider.tag;
        if (!audioRef.isPlaying)
        {
            switch (theTag)
            {
                case "Soil":
                    PlayRandomized(grass);
                    break;
                case "Water":
                    PlayRandomized(water);
                    break;
                case "Wood":
                    PlayRandomized(wood);
                    break;
                case "Rock":
                    PlayRandomized(rock);
                    break;
                case "Leaf":
                    PlayRandomized(leaf);
                    break;
            }
        }
    }
}
