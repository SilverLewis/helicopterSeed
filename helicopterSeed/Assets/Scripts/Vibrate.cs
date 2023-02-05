using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : MonoBehaviour
{
    [SerializeField]Transform start;
    [SerializeField]Transform end;

    [SerializeField] float length  = .5f;

    Transform trueStart;
    Transform trueEnd;

    bool rotating = false;
    bool rotateTowards = false;

    private void Start()
    {
        trueStart = start;
        trueEnd = end;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotating)
        {
            if (rotateTowards)
            {
                StartCoroutine(rotateTowardsFinal());
            }
            else {
                StartCoroutine(rotateFromFinal());
            }
        }
    }


    IEnumerator rotateTowardsFinal ()
    {
        rotating = true;
        for (float i = 0; i < length; i += .01f)
        {
            transform.rotation = Quaternion.Lerp(trueStart.rotation, trueEnd.rotation, i/length);
            yield return new WaitForSeconds(.01f);

        }
        rotateTowards = false;
        rotating = false;
    }

    IEnumerator rotateFromFinal()
    {
        rotating = true;
        for (float i = length; i >=0; i -= .01f)
        {
            transform.rotation = Quaternion.Lerp(trueEnd.rotation, trueStart.rotation, (length - i) / length);
            yield return new WaitForSeconds(.01f);

        }
        rotateTowards = true;
        rotating = false;
    }
}
