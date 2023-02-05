using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 currentRotation;

    [Space(5)]
    [Header("Wind")]
    [SerializeField]public Vector3 windDirection = new Vector3();
    [SerializeField]public float windPower = 0;

    [SerializeField] Vector3 internalVelocity = new Vector3();

    [Space(5)]
    [Header("Horizontal Movement")]

    [SerializeField] float internalMax = 10;
    [SerializeField] float acceleration = 5;

    public Vector3 forwardVector;

    [Space(5)]
    [Header("Rotation Values")]
    [SerializeField] float rotVel = 0;
    [SerializeField] float rotationAcceleration = 480;
    [SerializeField] float maxRotationSpeed = 1440;
    [SerializeField] float decelerationSpeed = 2880;

    [Space(5)]
    [Header("Rightening Values")]
    [SerializeField] float righteningSpeed = 45;


    [Space(5)]
    [Header("Vertical Speed")]
    [SerializeField] float gravity =-10;
    [SerializeField] float spinningterminalYVelocity = 5;
    [SerializeField] float terminalYVelocity = 10;
    float terminal = 10;
    float terminalDelta = 0;

    [Space(5)]
    [Header("Grounded Movement")]
    [SerializeField] float impulseForce=5;
    [SerializeField] float rotationTimerDelaySeconds = 1;
    [SerializeField] float jumpTimerDelaySeconds = 1;
    [SerializeField] float rightenTimerDelaySeconds = .5f;
    [Space(5)]
    [SerializeField] float currentRotationTimerDelaySeconds = 0;
    [SerializeField] float currentJumpTimerDelaySeconds = 0;
    [SerializeField] float currentRightenTimerDelaySeconds = 0;

    [Space(5)]
    [Header("Grounded Transforms")]
    [SerializeField] float groundedRaycastRange=.1f;
    [SerializeField] Transform[] corners;
    [SerializeField] bool onGround;
    Vector3[] cardinalVectors = new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };


    [Space(5)]
    [Header("Bar")]
    [SerializeField] GameObject BarHolder;
    onGroundBar barScript;


    // Start is called before the first frame update
    void Start()
    {
        float terminal = terminalYVelocity;
        rb = GetComponent<Rigidbody>();
        terminalDelta = terminalYVelocity - spinningterminalYVelocity;
        barScript = BarHolder.GetComponent<onGroundBar>();
    }

    private void FixedUpdate()
    {
        bool grounded = IsGrounded();
        if (!onGround && grounded)
        {
            currentJumpTimerDelaySeconds = jumpTimerDelaySeconds;
        }
        onGround = grounded;

        float spinRot = 0;

        Vector3 righten = new Vector3();
        if(CanRighten())
        {
            righten = Righten();
        }

        if (CanSpin())
        {
            Vector3 wallDirection = IsNextToWall();
            if (wallDirection == new Vector3())
            {
                spinRot = GetYRotation(grounded);
            }
            else {
                rotVel = 0;
            }
        }
        
        if (spinRot > 0 && righten != new Vector3())
        {
            rb.MoveRotation(rb.rotation* Quaternion.Euler(new Vector3(0, spinRot, 0)) * Quaternion.Euler(righten));
        }
        else if (spinRot > 0)
        {
            rb.MoveRotation(rb.rotation* Quaternion.Euler(new Vector3(0, spinRot, 0)));
        }
        else if ( righten != new Vector3())
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(righten));
        }

        CacluateTerminalVelocity();
        
        //add gravity
        rb.AddForce(Vector3.up* gravity, ForceMode.Force);
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Max(rb.velocity.y, terminal), rb.velocity.z);

        //add wind
        rb.AddForce(windDirection*windPower, ForceMode.Force);

        //add internal
        rb.AddForce(internalVelocity, ForceMode.Force);
        Vector3 forwardMax = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized*internalMax;
        //we abs to make sure we can use min then return the sign)
        rb.velocity = new Vector3(
            Mathf.Sign(rb.velocity.x) * Mathf.Min(Mathf.Abs(forwardMax.x), Mathf.Abs(rb.velocity.x)),
            rb.velocity.y,
            Mathf.Sign(rb.velocity.z) * Mathf.Min(Mathf.Abs(forwardMax.z), Mathf.Abs(rb.velocity.z))
            );

    }

    // Update is called once per frame
    void Update()
    {
        TickTimers();

        float forward = Input.GetAxis("Vertical") ;
        float horizontal = Input.GetAxis("Horizontal") ;
        
        var rightVector = new Vector3(forwardVector.z, 0, -forwardVector.x);

        Vector3 movementVector = forward * forwardVector + rightVector * horizontal;

        movementVector =Vector3.Normalize(movementVector);

        if (!onGround)
        {
            if (forward != 0 || horizontal != 0)
            {
                //float currentSpinRatio = rotVel / maxRotationSpeed;
                internalVelocity = Vector3.ClampMagnitude(internalVelocity + movementVector * acceleration * Time.deltaTime, internalMax);
            }
            else {
                internalVelocity = Vector3.ClampMagnitude(internalVelocity - internalVelocity * acceleration * Time.deltaTime, internalMax);
            }
        }
        else if(rotVel<=0 && Vector3.Magnitude(movementVector)>0 && CanJump()) {
            Jump(movementVector);
        }
    }

    private float GetYRotation(bool grounded)
    {
        float rotationSpeedIncreasePerSecond = 0;

        if (grounded) {
            if (rotVel > 0)
            {
                rotVel -= decelerationSpeed * Time.deltaTime;
                rotVel = Mathf.Max(0, rotVel);
            }
            else {
                rotVel += decelerationSpeed * Time.deltaTime;
                rotVel = Mathf.Min(0, rotVel);
            }
        }
        else {
            rotVel = Mathf.Min(rotVel + (rotationAcceleration * Time.deltaTime), maxRotationSpeed);
        }

        rotationSpeedIncreasePerSecond = rotVel * Time.deltaTime;
        return rotationSpeedIncreasePerSecond;
    }

    private void CacluateTerminalVelocity()
    {
        float spinningRatio = rotVel / maxRotationSpeed;
        terminal = -(terminalYVelocity - (terminalDelta * spinningRatio)) * Time.deltaTime;
    }

    private bool IsGrounded()
    {
        for (int i =0;i<corners.Length;i++)
        {
            if (Physics.Raycast(corners[i].position, -Vector3.up, groundedRaycastRange))
            {
                barScript.Grounded();
                return true;
            }
        }
        barScript.AirBorn();
        return false;
    }
   
    private Vector3 IsNextToWall() {

        for (int i = 0; i < corners.Length; i++)
        {
            for(int j = 0; j < cardinalVectors.Length; j++) { 
                if (Physics.Raycast(corners[i].position, cardinalVectors[j], groundedRaycastRange*2))
                {
                      print("here");
                    return cardinalVectors[j];
                }
            }
        }
        return new Vector3();
    }

    void Jump(Vector3 movementVector)
    {
        currentJumpTimerDelaySeconds = jumpTimerDelaySeconds;
        currentRotationTimerDelaySeconds = rotationTimerDelaySeconds;
        currentRightenTimerDelaySeconds = rightenTimerDelaySeconds;

        rotVel = 0;

        movementVector.y += 1;
        movementVector = Vector3.Normalize(movementVector);
        movementVector *= impulseForce;
        rb.AddForceAtPosition(movementVector, transform.position, ForceMode.Impulse);
    }

    void TickTimers()
    {
        if (currentRotationTimerDelaySeconds > 0)
        {
            currentRotationTimerDelaySeconds -= Time.deltaTime;
        }
        if (currentJumpTimerDelaySeconds > 0)
        {
            currentJumpTimerDelaySeconds -= Time.deltaTime;
        }
        if (currentRightenTimerDelaySeconds > 0)
        {
            currentRightenTimerDelaySeconds -= Time.deltaTime;
        }
    }

    bool CanSpin() {
        return currentRotationTimerDelaySeconds <= 0;
    }

    bool CanRighten()
    {
        return currentRightenTimerDelaySeconds <= 0;
    }

    bool CanJump()
    {
        return currentJumpTimerDelaySeconds <= 0;
    }

    Vector3 Righten() {
        Vector3 rotation = rb.rotation.eulerAngles;
        float Xrot =Mathf.Abs( rotation.x % 180);
        float Zrot =Mathf.Abs( rotation.z % 180);
        rotation.y = 0;

        float rightenPerSec = righteningSpeed * Time.deltaTime;

        
        if (Xrot != 0 )
        {
            float step = 0;
            if (Xrot > 90)
            {
                step = Mathf.Min(rightenPerSec, 180 - Xrot);
            }
            else {
                step -= Mathf.Min(rightenPerSec, Xrot);
            }
            rotation.x = step * Mathf.Sign(rotation.x);
        }

        if (Zrot != 0)
        {
            float step = 0;
            if (Zrot > 90)
            {
                step = Mathf.Min(rightenPerSec, 180 - Zrot);
            }
            else
            {
                step -= Mathf.Min(rightenPerSec, Zrot);
            }
            rotation.z = step * Mathf.Sign(rotation.z);
        }

        return rotation;
    }
}
