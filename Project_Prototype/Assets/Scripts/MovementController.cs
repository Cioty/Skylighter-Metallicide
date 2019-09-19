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

    // Jump stuff.
    public float jumpForce = 10.0f;
    public float distanceToGround = 1.5f;
    private bool canJump = true;
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        // Setting the body to the charaters rigidbody.
        this.body = this.GetComponent<Rigidbody>();
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
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }
}
