using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] GameObject visualFront,visualBack;
    AudioSource audioRef;
    bool got;
    public static int coinCount=0;
    // Start is called before the first frame update
    void Start()
    {
        audioRef = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, spinSpeed * Time.deltaTime, 0));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !got)
        {
            got = true;
            //add to the coin counter
            //play a sound
            audioRef.Play();
            coinCount++;
            visualFront.SetActive(false);
            visualBack.SetActive(false);
            Destroy(transform.parent.gameObject, 1f);
        }
    }
}
