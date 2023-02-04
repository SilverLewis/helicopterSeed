using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform cameraPivot;

    [Space(5)]
    [Header("Seed")]
    [SerializeField] Transform seedPivot;

    [Space(5)]
    [SerializeField] bool cameraMovementEnabled = true;
    [SerializeField] float horizontalRotationSpeed=30;
    [SerializeField] float verticalRotationSpeed=30;
    [SerializeField] float cameraMaxVerticalDegrees = 75;
    [SerializeField] float cameraMinVerticalDegrees = 15;





    // Start is called before the first frame update
    void Start()
    {
        cameraPivot = transform.transform;
    }

    private void FixedUpdate()
    {
        cameraPivot.transform.position = seedPivot.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraMovementEnabled) { 
            float verticalRotationChange = Input.GetAxis("Vertical") * Time.deltaTime * verticalRotationSpeed;
            float horizontalRotationChange = -Input.GetAxis("Horizontal") * Time.deltaTime * horizontalRotationSpeed;

           Vector3 rotation =cameraPivot.rotation.eulerAngles;

            rotation.x = Mathf.Clamp(verticalRotationChange + rotation.x, cameraMinVerticalDegrees, cameraMaxVerticalDegrees);
            rotation.y += horizontalRotationChange;

           cameraPivot.rotation =  Quaternion.Euler(rotation);
        }
    }
}
