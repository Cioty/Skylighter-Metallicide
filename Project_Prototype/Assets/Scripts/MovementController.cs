using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // References.
    private Rigidbody body;
    private Vector3 moveDirection;

    [Header("Attributes")]
    // General movement.
    public float movementSpeed = 10.0f;

    [Header("Dashing parameters")]
    // Dashing stuff
    public float movementSpeedMin;
    public float dashMultiplier = 2.0f;
    private float dashSpeedMax;
    private float acceleration;

    [Header("Jumping parameters")]
    // Jump stuff.
    public float jumpForce = 10.0f;
    public float distanceToGround = 1.5f;
    private bool canJump = true;
    private bool isJumping = false;

    // Particle stuff for Dash
    public GameObject dashEffect;

    // Time
    private float startTime;

    private void Awake()
    {
        startTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setting the body to the charaters rigidbody.
        this.body = this.GetComponent<Rigidbody>();

        // Stores the original movement Speed
        movementSpeedMin = movementSpeed;
        dashSpeedMax = movementSpeed * dashMultiplier;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            isJumping = true;
            canJump = false;
        }

        if (!canJump)
        {
            if (IsGrounded())
                canJump = true;
        }

        // Dashing function: Hold right shift to dash
        isDashing();

        // Movement.
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * movementSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (isJumping && canJump)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
    }
    // Checking if the player is grounded via raycast.
    private bool IsGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -Vector3.up);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject targetHit = hit.transform.gameObject;
            if (targetHit && targetHit.tag == "Ground" && hit.distance <= distanceToGround)
            {
                Debug.Log("true: " + hit.distance);
                return true;
            }
            else
            {
                Debug.Log("false: " + hit.distance);
                return false;
            }
        }
        else
            return false;
    }

    // Dashing function
    private void isDashing()
    {
        // Dashing
        if (Input.GetKey(KeyCode.RightShift))
        {
            // This updates the acceleration every frame if right shift is down.
            acceleration = dashSpeedMax - movementSpeed;
            if (movementSpeed < dashSpeedMax)
            {
                movementSpeed += (acceleration) / Time.time - startTime;
            }

            if (movementSpeed > dashSpeedMax)
            {
                // Movement Speed is set to what the max Dash is
                movementSpeed = dashSpeedMax;
            }
        }
        else
        {
            // This gets the deceleration rate
            acceleration = movementSpeedMin - movementSpeed;
            if (movementSpeed > movementSpeedMin)
            {
                movementSpeed += (acceleration) / Time.time - startTime;
            }

            if (movementSpeed < movementSpeedMin)
            {
                // Sets it back to the original movement speed
                movementSpeed = movementSpeedMin;
            }
        }
    }
 
}
