using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController : MonoBehaviour
{
    [Header("References")]
    public GameObject playerObject;
    public PlayerHandler playerHandler;
    private CharacterController controller;

    [Header("Movement")]
    public float speed = 5.0f;
    public float gravity = 10.0f;
    public float groundDrag = 20.0f;
    public float jumpForce = 6.0f;
    public float airAccelerationSpeed;
    
    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private float horizontalAxis, verticalAxis;

    // Start is called before the first frame update
    void Start()
    {
        // Setting the cursors lock state.
        Cursor.lockState = CursorLockMode.Locked;

        // Get the character controller in the player.
        controller = playerObject.GetComponent<CharacterController>();
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
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");
        moveDirection = (horizontalAxis * playerObject.transform.right) + (verticalAxis * playerObject.transform.forward);
        moveDirection.Normalize();
    }

    private void FixedUpdate()
    {
        // Update the move vector.
        this.UpdateMoveVector();

        if (controller.isGrounded)
        {
            // Calculating fixed movement.
            currentVelocity = moveDirection * speed;

            // Adding ground drag.
            currentVelocity.y -= groundDrag;

            // Checking for jump input.
            if (Input.GetButton("Jump"))
                currentVelocity = (playerObject.transform.forward + moveDirection * jumpForce) + Vector3.up * jumpForce;
        }
        else
        {
            // Calculating gravity.
            Vector3 gravityVector = Vector3.down * gravity * Time.deltaTime;
            currentVelocity += gravityVector;

            // Calculating the in air movement.
            Vector3 inAirMoveVector = moveDirection * speed;
            inAirMoveVector -= currentVelocity;

            // Applying the air movement and gravity vectors.
            Vector3 velocityDiff = Vector3.ProjectOnPlane(inAirMoveVector, gravityVector);
            currentVelocity += velocityDiff * airAccelerationSpeed * Time.deltaTime;
        }

        playerHandler.CurrentVelocity = currentVelocity;

        // Moving the player with the calculated velocity.
        controller.Move(currentVelocity * Time.deltaTime);
    }

}
