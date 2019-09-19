using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody body;
    private Vector3 moveDirection;

    [Header("Attributes")]
    public float movementSpeed = 10.0f;
    public float jumpForce = 10.0f;
    public float distanceToGround = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Setting the body to the charaters rigidbody.
        this.body = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
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
        // Jumping.
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    // Checking if the player is grounded.
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
}
