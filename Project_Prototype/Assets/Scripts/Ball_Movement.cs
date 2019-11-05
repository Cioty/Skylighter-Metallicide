using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Ball_Movement : MonoBehaviour
{
    // References
    public GameObject coreObject;
    public Rigidbody rigidBody;
    public SphereCollider sphereCollider;
    public GameObject thirdPersonCamera;
    public PlayerHandler playerHandler;

    // Movement
    public float movementSpeed = 5.0f;

    // Jumping 
    public float jumpForce = 5.0f;
    private bool isJumping = false;

    // Collider variables
    public float distanceToGround = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
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
        cam_forward = cam_forward.normalized;
        cam_right = cam_right.normalized;

        Vector3 movement = (cam_forward * vertical_move + cam_right * horizontal_move);

        rigidBody.AddForce(movement * movementSpeed);
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
