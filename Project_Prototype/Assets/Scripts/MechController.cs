﻿/*=============================================================================
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
    private GameObject playerObject;
    private PlayerHandler playerHandler;
    private CharacterController controller;

    [Header("Movement")]
    public float maxSpeed = 25.0f;
    public float accelerationMultiplier = 1.0f;
    public float decelerationMultiplier = 1.0f;
    public float gravity = 10.0f;
    //public float groundDrag = 20.0f;
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

    // Start is called before the first frame update
    private void Start()
    {
        // Setting the cursors lock state.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake()
    {
        // Gets the required compents from the player object.
        playerObject = this.gameObject.transform.parent.gameObject;
        playerHandler = GetComponent<PlayerHandler>();
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
            // If the player is inputting a direction:
            if (moveDirection.magnitude > 0)
            {
                deccelTimer = 0.0f;
                accelTimer += accelerationRate.Evaluate(Time.fixedDeltaTime * accelerationMultiplier);
                acceleration = Vector3.Lerp(acceleration, moveDirection * maxSpeed, accelTimer / 1.0f);
            }
            else
            {
                // If there is no user input, then lerp the acceleration to 0 by the deceleration rate.
                accelTimer = 0.0f;
                deccelTimer += accelerationRate.Evaluate(Time.fixedDeltaTime * decelerationMultiplier);
                acceleration = Vector3.Lerp(acceleration, new Vector3(0, 0, 0), deccelTimer / 1.0f);
            }

            // Applying direction and acceleration to the current velocity.
            currentVelocity = lastDirection + acceleration;
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, 25);
            Debug.Log(currentVelocity);

            // Removed ground drag.
            //currentVelocity.y -= groundDrag;

            // Checking for jump input.
            if (Input.GetButton("Jump") || XCI.GetButtonDown(XboxButton.A, playerHandler.AssignedController))
            {
                currentVelocity = (playerObject.transform.forward + moveDirection * jumpHeight) + acceleration.normalized + Vector3.up * jumpHeight;

                // Currently just resetting the acceleration after using it for the jump, need a fix for a bug here.
                acceleration = Vector3.zero;
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
        }

        // Keeping track of the current velocity via the player handler.
        playerHandler.CurrentVelocity = currentVelocity;

        // Moving the player with the calculated velocity.
        controller.Move(playerHandler.CurrentVelocity * Time.deltaTime);
    }

    public void Move(Vector3 motion)
    {
        controller.Move(motion * Time.deltaTime);
    }

}
