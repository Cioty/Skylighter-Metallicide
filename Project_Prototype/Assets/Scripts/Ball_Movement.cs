using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Ball_Movement : MonoBehaviour
{
    // References
    public GameObject thirdPersonCamera;
    public GameObject sphereObject;
    public GameObject coreObject;
    public GameObject coreCage;

    // Getting the rigidbody and collider from the sphere object
    private SphereCollider sphereCollider;
    private Rigidbody rb;
    private PlayerHandler playerHandler;

    // Movement
    private float currentSpeed;
    public float movementSpeed = 5.0f;
    public float maxSpeed = 10.0f;
    public float maxAngularVelocity = 25.0f;
    private Vector3 gravity = new Vector3(0.0f, -9.8f);
    //private float acceleration = 0.0f;

    //private float smooth = 1.0f;

    // For steering behaviour
    Vector3 desiredVelocity;
    Vector3 currentVelocity;

    Vector3 desiredDirection = Vector3.zero;
    Vector3 currentDirection = Vector3.zero;

    // Cage rotation
    Quaternion currentCageRot;
    Quaternion newCageRot;


    // Jumping 
    public float jumpForce = 5.0f;
    private bool isJumping = false;

    private bool isGrounded;

    // Collider variables
    public float distanceToGround = 0.5f;

    private void Awake()
    {
        rb = sphereObject.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
        sphereCollider = sphereObject.GetComponent<SphereCollider>();
        playerHandler = GetComponentInParent<PlayerHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {        
        currentVelocity = Vector3.zero;
        currentCageRot = new Quaternion(coreCage.transform.localRotation.x, 0.0f, 0.0f, 0.0f);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded() && playerHandler.IsControllable)
        {
            isJumping = true;
        }

        if (!IsGrounded())
        {
            rb.AddForce(-Vector3.up * 10.0f, ForceMode.Acceleration);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ball_roll();
        coreDirection();

        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
    }


    // Making the ball move in the direction the camera is facing
    void ball_roll()
    {
        float horizontal_move = 0f;
        float vertical_move = 0f;
        if(playerHandler.HasAssignedController)
        {
            horizontal_move = XCI.GetAxis(XboxAxis.LeftStickX, playerHandler.AssignedController);
            vertical_move = XCI.GetAxis(XboxAxis.LeftStickY, playerHandler.AssignedController);
        }
        else
        {
            horizontal_move = Input.GetAxis("Horizontal");
            vertical_move = Input.GetAxis("Vertical");
        }

        // So the ball moves where the Camera faces
        Vector3 cam_forward = thirdPersonCamera.transform.forward;
        Vector3 cam_right = thirdPersonCamera.transform.right;

        // Locking camera's y rotation to 0, so it doesn't tilt away from the forward axis.
        cam_right.y = 0.0f;
        cam_forward.y = 0.0f;
       

        // The direction
        desiredDirection = (cam_forward * vertical_move + cam_right * horizontal_move).normalized;

        // Check if there's input
        if (desiredDirection.magnitude != 0.0f)
        {           
                // Acceleration achieved through dividing
                currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, currentSpeed + movementSpeed / maxSpeed);

                if (currentSpeed > maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }          
        }
        else
        {
            currentSpeed = 0.0f;
        }

        //coreSteer(currentDirection, desiredDirection, currentSpeed);
        currentVelocity = (desiredDirection) * currentSpeed;

        // Setting the core's cage to the direction of the camera
        if (vertical_move != 0.0f)
            coreCage.transform.localRotation = coreCage.transform.localRotation * Quaternion.AngleAxis(coreCage.transform.localRotation.x + currentSpeed, Vector3.right);

        // Translate position
        rb.AddForce(currentVelocity );

        // Will always be where the player faces.        
    }

    void coreDirection()
    {
        // Get the new direction the player is facing
        newCageRot = thirdPersonCamera.transform.rotation;

        // The core's inner rotation equal to Camera
        coreObject.transform.localRotation = Quaternion.Slerp(currentCageRot, newCageRot, Time.deltaTime * 10.0f);

        currentCageRot = coreObject.transform.rotation;
    }


    /// <summary>
    /// Takes time for the Core steer in the desired direction
    /// </summary>
    /// <param name="currentDir"> They direction the ball moves in for that frame </param>
    /// <param name="desiredDir"> Where the Core should go </param>
    /// <param name="speed"> Makes the Core move at this speed </param>
    void coreSteer(Vector3 currentDir, Vector3 desiredDir, float speed)
    {
        // Desired velocity that the Core will move to
        desiredVelocity = desiredDir * speed;

        // Get the difference between the desired and current velocity
        Vector3 steeringForce = desiredVelocity - currentVelocity;

        // Current velocity slowly becomes the desired velocity
        currentVelocity = (currentDir + steeringForce) * speed;
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(sphereCollider.transform.position, -Vector3.up);

        if (Physics.Raycast(ray, out hit, distanceToGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Entered");
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("Exit");
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}
}
