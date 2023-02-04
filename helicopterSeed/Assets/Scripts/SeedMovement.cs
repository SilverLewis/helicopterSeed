using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 currentRotation;





    [Space(5)]
    [Header("Horizontal Movement")]
    [SerializeField] float fmod = 1;
    [SerializeField] float hmod = 1;


    [Space(5)]
    [Header("Rotation Values")]
    [SerializeField] float currentRotationSpeed = 0;
    [SerializeField] float rotationAcceleration = 480;
    [SerializeField] float maxRotationSpeed = 1440;

    [Space(5)]
    [Header("Vertical Speed")]
    [SerializeField] float gravity = 10;
    [SerializeField] float maxFallingSpeed = 10;
    [SerializeField] float maxSpinningFallingSpeed = 5;
    float maxFallingSpeedDelta = 0;


    // Start is called before the first frame update
    void Start()
    {
        maxFallingSpeedDelta = maxFallingSpeed- maxSpinningFallingSpeed; 
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(GetNewRotation()));
        AddFallingSpeed();
    }
    // Update is called once per frame
    void Update()
    {
        float forward = Input.GetAxis("Vertical") * Time.deltaTime*fmod;
        float horizontal = Input.GetAxis("Horizontal")  *Time.deltaTime*hmod;
        
        rb.velocity = new Vector3(forward, rb.velocity.y,horizontal);
    }

    private Vector3 GetNewRotation() {
        currentRotationSpeed = Mathf.Min(currentRotationSpeed + (rotationAcceleration * Time.deltaTime), maxRotationSpeed);
        float rotationSpeedPerSecond = currentRotationSpeed * Time.deltaTime;

        currentRotation = rb.rotation.eulerAngles;

        return (new Vector3(0, rotationSpeedPerSecond, 0) + currentRotation);
    }

    private void AddFallingSpeed()
    {
        float spinningRatio = currentRotationSpeed / maxRotationSpeed;
        float currentMaxFallingSpeed = -(maxFallingSpeed + (maxFallingSpeedDelta * spinningRatio))*Time.deltaTime;
        
        float currentFallingSpeed = rb.velocity.y;

        if (currentMaxFallingSpeed >= currentFallingSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, currentMaxFallingSpeed, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, currentFallingSpeed-gravity*Time.deltaTime, rb.velocity.z);
        }

    }
}
