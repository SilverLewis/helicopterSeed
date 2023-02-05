using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// rotates an object to always have its forward point to the camera
/// </summary>
public class FaceTheCamera : MonoBehaviour
{
    Camera cam;
    [SerializeField] bool verticalTurn = false;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        FaceCamera();
    }
    void FaceCamera()
    {
        if (verticalTurn)
        {
            transform.LookAt(cam.transform.position);

        }
        else
        {
            transform.LookAt(new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z));
        }
    }
}
