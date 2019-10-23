/*=============================================================================
 * Game:        Metallicide
 * Version:     Alpha
 * 
 * Class:       MechController.cs
 * Purpose:     To control the movement of the mech.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using UnityEngine;
using XboxCtrlrInput;

public class MechController : MonoBehaviour
{
    //private GameObject playerObject;
    private PlayerHandler playerHandler;
    private Transform mechObjectTransform;
    private Rigidbody rb;
    private float distanceToGround;

    [Header("References")]
    public CapsuleCollider capsuleCollider;
    public GameObject mechObject;

    [Header("Movement")]
    public float maxSpeed = 25.0f;
    public float accelerationMultiplier = 1.0f;
    public float decelerationMultiplier = 1.0f;
    public float gravity = 10.0f;
    public float jumpHeight = 6.0f;
    public float airAccelerationSpeed = 1.0f;

    [Header("Curves")]
    public AnimationCurve accelerationRate;
    public AnimationCurve decelerationRate;


    // Private variables.
    private Vector3 acceleration = Vector3.zero;
    private Vector3 deceleration = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lastDirection = Vector3.zero;
    private Vector3 currentVelocity;
    private float horizontalAxis, verticalAxis;
    private float accelTimer, deccelTimer, directionalTimer;

    void Awake()
    {
        playerHandler = GetComponentInParent<PlayerHandler>();
        rb = GetComponent<Rigidbody>();
        mechObjectTransform = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Setting the cursors lock state.
        Cursor.lockState = CursorLockMode.Locked;
        distanceToGround = capsuleCollider.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Checking if we want to unlock the mouse.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Gets the axis values then applys it to the movement vector with the players direction.
    private void UpdateMoveVector()
    {
        if(playerHandler.AssignedController > 0)
        {
            horizontalAxis = XCI.GetAxis(XboxAxis.LeftStickX, playerHandler.AssignedController);
            verticalAxis = XCI.GetAxis(XboxAxis.LeftStickY, playerHandler.AssignedController);
        }
        else
        {
            horizontalAxis = Input.GetAxisRaw("Horizontal");
            verticalAxis = Input.GetAxisRaw("Vertical");
        }
        moveDirection = (horizontalAxis * transform.right) + (verticalAxis * transform.forward);
        moveDirection.Normalize();
    }

    private void FixedUpdate()
    {
        // Update the move vector.
        this.UpdateMoveVector();

        bool grounded = IsGrounded();
        playerHandler.IsGrounded = grounded;

        // If the player is on the ground:
        if (grounded)
        {
            // If the player is inputting a direction:
            if (moveDirection.magnitude > 0)
            {
                deccelTimer = 0.0f;
                accelTimer += accelerationRate.Evaluate(accelerationMultiplier * Time.fixedDeltaTime);
                acceleration = Vector3.Lerp(acceleration, moveDirection * maxSpeed, accelTimer / 1.0f);
            }
            else
            {
                // If there is no user input, then lerp the acceleration to 0 by the deceleration rate.
                accelTimer = 0.0f;
                deccelTimer += accelerationRate.Evaluate(decelerationMultiplier * Time.fixedDeltaTime);
                acceleration = Vector3.Lerp(acceleration, new Vector3(0, 0, 0), deccelTimer / 1.0f);
            }

            // Applying direction and acceleration to the current velocity.
            currentVelocity = acceleration;
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, 25);

            // Checking for jump input.
            if (XCI.GetButton(XboxButton.A, playerHandler.AssignedController) || Input.GetButton("Jump"))
            {
                currentVelocity += (acceleration.normalized + Vector3.up) * jumpHeight;
            }
        }
        else
        {
            // Calculating gravity.
            Vector3 gravityVector = Vector3.down * gravity * Time.deltaTime;
            currentVelocity += gravityVector;

            // Calculating the in air movement.
            Vector3 inAirMoveVector = moveDirection * maxSpeed;
            inAirMoveVector -= currentVelocity;

            // Applying the air movement and gravity vectors.
            Vector3 velocityDiff = Vector3.ProjectOnPlane(inAirMoveVector, gravityVector);
            currentVelocity += velocityDiff * airAccelerationSpeed * Time.deltaTime;

            // Setting acceleration to be the velocity to prevent boosted movement on ground.
            acceleration = currentVelocity;
        }

        // Keeping track of the current velocity via the player handler.
        playerHandler.CurrentVelocity = currentVelocity;

        //Debug.Log(currentVelocity);

        // Making the rigid bodies velocity equal the calculated velocity.
        rb.velocity = currentVelocity;
    }

    private bool IsGrounded()
    {
        // Correcting the start position of the ray.
        Vector3 startPos = transform.position;
        startPos.y += 1.8f;

        // Returning a condition if we hit the ground or not.
        return (Physics.Raycast(new Ray(startPos, -Vector3.up), distanceToGround + 0.1f, LayerMask.NameToLayer("PlayerMovement")));
    }
}
