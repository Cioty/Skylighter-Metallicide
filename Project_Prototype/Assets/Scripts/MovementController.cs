using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // References.
    private Rigidbody body;

	[Header("Attributes")]
	// General movement.
	public float movementSpeed = 10.0f;
	public float moveAcceleration = 5.0f;
	private Vector3 moveDirection = new Vector3();
	private Vector3 moveVelocity = new Vector3();


    [Header("Dashing")]
    // Dashing stuff
    public float movementSpeedMin;
    public float dashMultiplier = 2.0f;
    private float dashSpeedMax;
    private float dashAcceleration;

    [Header("Jumping")]
    // Jump stuff.
    public float jumpForce = 10.0f;
    public float distanceToGround = 1.5f;
    private bool canJump = true;

	[Header("Dashing")]
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
        // Checking if the character can jump again.
        if (!canJump)
        {
            if (IsGrounded())
                canJump = true;
        }

        // Dashing function: Hold right shift to dash
        isDashing();
	}

    private void FixedUpdate()
    {

		/*
		 * TODO:
		 *		- Add acceleration, max speed, turn speed and air control!
		 */

		// Resetting the movement vectors.
		moveDirection = Vector3.zero;
		moveVelocity = Vector3.zero;

		// Movement keybinds.
		if (Input.GetKey(KeyCode.W))
		{
			moveDirection += transform.forward * movementSpeed;
		}
		if (Input.GetKey(KeyCode.A))
		{
			moveDirection += -transform.right * movementSpeed;
		}
		if (Input.GetKey(KeyCode.S))
		{
			moveDirection += -transform.forward * movementSpeed;
		}
		if (Input.GetKey(KeyCode.D))
		{
			moveDirection += transform.right * movementSpeed;
		}

		// Normalising the direction vector.
		moveDirection.Normalize();

		// Moving the data into the velocity vector to maintain the rb y pos.
		moveVelocity.x = moveDirection.x;
		moveVelocity.y = body.velocity.y;
		moveVelocity.z = moveDirection.z;

		// Making the rb velocity the calculated velocity.
		body.velocity = moveVelocity;

		// Jumping.
		if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			canJump = false;
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
                return true;
            else
                return false;
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
            dashAcceleration = dashSpeedMax - movementSpeed;
            if (movementSpeed < dashSpeedMax)
            {
                movementSpeed += (dashAcceleration) / Time.time - startTime;
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
            dashAcceleration = movementSpeedMin - movementSpeed;
            if (movementSpeed > movementSpeedMin)
            {
                movementSpeed += (dashAcceleration) / Time.time - startTime;
            }

            if (movementSpeed < movementSpeedMin)
            {
                // Sets it back to the original movement speed
                movementSpeed = movementSpeedMin;
            }
        }
    }
}
