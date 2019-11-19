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
    //private float acceleration = 0.0f;

    //private float smooth = 1.0f;

    Vector3 currentVelocity;
    Vector3 direction;

    // Cage rotation
    Quaternion currentCageRotX;
    Quaternion newCageRotX;

    // Jumping 
    public float jumpForce = 5.0f;
    private bool isJumping = false;

    // Collider variables
    public float distanceToGround = 0.5f;

    private void Awake()
    {
        rb = sphereObject.GetComponent<Rigidbody>();
        sphereCollider = sphereObject.GetComponent<SphereCollider>();
        playerHandler = GetComponentInParent<PlayerHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //currentCageRotX = new Quaternion(coreCage.transform.localRotation.x, 0.0f, 0.0f, 0.0f);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded() && playerHandler.IsControllable)
        {
            isJumping = true;
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
        cam_forward.y = 0.0f;
        cam_right.y = 0.0f;
        cam_forward = cam_forward.normalized;
        cam_right = cam_right.normalized;

        // The direction
        direction = (cam_forward * vertical_move + cam_right * horizontal_move);

        // Check if there's input
        if (direction.magnitude != 0.0f)
        {
            // Acceleration achieved through dividing
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, movementSpeed/maxSpeed);

            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }
        else
        {           
            // Deceleration achieved through dividing current speed by max speed
            currentSpeed = Mathf.Lerp(currentSpeed, 0.0f, movementSpeed/maxSpeed);

            if (currentSpeed < 0.0f)
            {
                currentSpeed = 0.0f;
            }
        }

        currentVelocity = direction * currentSpeed;
        // coreCage.transform.localRotation = coreCage.transform.localRotation * Quaternion.AngleAxis(coreCage.transform.localRotation.x + currentSpeed/movementSpeed, Vector3.right);

        // Translate position
        rb.AddForce(currentVelocity); 
    }

    void coreDirection()
    {
        // The core's inner rotation from Camera
        coreObject.transform.localRotation = thirdPersonCamera.transform.localRotation;
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -Vector3.up);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject targetHit = hit.transform.gameObject;
            if (targetHit && targetHit.tag == "Ground" && hit.distance <= distanceToGround)
            {
                Debug.Log("hit: " + hit.distance);
                return true;
            }
            else
            {
                Debug.Log("no: " + hit.distance);
                return false;
            }
        }
        else
            return false;
    }
}
