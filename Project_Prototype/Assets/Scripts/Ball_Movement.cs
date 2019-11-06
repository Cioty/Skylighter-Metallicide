using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Ball_Movement : MonoBehaviour
{
    // References
    [Header("This object's children components")]
    public GameObject coreObject;
    public Rigidbody rigidBody;
    public SphereCollider sphereCollider;

    public GameObject thirdPersonCamera;
    private PlayerHandler playerHandler;

    // Movement
    [Header("Movement")]
    public float movementSpeed = 5.0f;
    private float currentSpeed;
    public float maxSpeed = 10.0f;

    // Creating acceleration
    Vector3 currentVelocity = Vector3.zero;
    Vector3 newVelocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    Vector3 movementDirection;

    // Jumping 
    public float jumpForce = 5.0f;
    private bool isJumping = false;

    // Collider variables
    public float distanceToGround = 0.5f;
  
    private void Start()
    {
        playerHandler = GetComponentInParent<PlayerHandler>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if(playerHandler.HasAssignedController)
        {
            if (XCI.GetButton(XboxButton.A, playerHandler.AssignedController) && IsGrounded())
            {
                isJumping = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                isJumping = true;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ball_roll();

        if (isJumping)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
    }


    // Making the ball move in the direction the camera is facing
    void ball_roll()
    {
        float horizontal_move = 0f;
        float vertical_move = 0f;

        if (playerHandler.HasAssignedController)
        {
            horizontal_move = XCI.GetAxis(XboxAxis.LeftStickX, playerHandler.AssignedController);
            vertical_move = XCI.GetAxis(XboxAxis.LeftStickY, playerHandler.AssignedController);
        }
        else
        {
            horizontal_move = Input.GetAxis("Horizontal");
            vertical_move = Input.GetAxis("Vertical");
        }

        Vector3 cam_forward = thirdPersonCamera.transform.forward;
        Vector3 cam_right = thirdPersonCamera.transform.right;

        cam_forward.y = 0.0f;
        cam_right.y = 0.0f;
        
        // Getting the Ball's movement direction
        movementDirection = (cam_forward * vertical_move + cam_right * horizontal_move);
        movementDirection.Normalize();

        // Check for player input
        if (movementDirection.magnitude > 0)
        {
            if (currentSpeed < maxSpeed)
                currentSpeed += (maxSpeed - movementSpeed);
            else
            {
                currentSpeed = maxSpeed;
            }

            acceleration = Vector3.Lerp(acceleration, movementDirection * maxSpeed, currentSpeed / maxSpeed);
        }
        else
        {
            if (currentSpeed > 0)
                currentSpeed -= (maxSpeed - movementSpeed);
            else
                currentSpeed = 0.0f;

            acceleration = Vector3.Lerp(acceleration, Vector3.zero, currentSpeed / maxSpeed);
        }

        currentVelocity = acceleration;
        currentVelocity = Vector3.ClampMagnitude(acceleration, maxSpeed);

        rigidBody.AddForce(currentVelocity);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(coreObject.transform.position, -Vector3.up);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject targetHit = hit.transform.gameObject;
            if (hit.distance <= distanceToGround)
                return true;
            else
                return false;
        }
        else
            return false;
    }
}
