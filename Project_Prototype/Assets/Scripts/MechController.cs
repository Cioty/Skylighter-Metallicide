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
using UnityEditor;
using UnityEngine;
using XboxCtrlrInput;

public class MechController : MonoBehaviour
{
    [Header("Animator Properties")]
    public Animator mech_Animator;
    public Animator sheild_Animator;
    public float walkingVelocityPadding = 0.2f;
    public float walkSpeedMultiplier = 1.0f;
    public float timeToMaxWalkSpeed = 2.0f;

    [Header("Movement Attributes")]
    public float maxSpeed = 25.0f;
    public float accelerationMultiplier = 1.0f;
    public float decelerationMultiplier = 1.0f;
    public float gravity = 10.0f;
    public float jumpHeight = 6.0f;
    public float airAccelerationSpeed = 1.0f;
    public float maxVelocity = 25f;

    [Header("Curves")]
    public AnimationCurve accelerationRate;
    public AnimationCurve decelerationRate;

    // Private variables.
    private PlayerHandler playerHandler;
    private Transform mechObjectTransform;
    private CharacterController controller;
    private float distanceToGround;
    private Vector3 acceleration = Vector3.zero;
    private Vector3 deceleration = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lastDirection = Vector3.zero;
    private Vector3 gravityVector = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;
    private bool shouldResetGravity = false;
    private float horizontalAxis, verticalAxis;
    private float accelTimer, deccelTimer, directionalTimer;
    private bool justBoosted = false;
    private bool justJumped = false;

    // Double/rocket Jump
    private RocketJump rocketJump;

    void Awake()
    {
        mechObjectTransform = GetComponent<Transform>();
        playerHandler = GetComponentInParent<PlayerHandler>();
        controller = mechObjectTransform.GetComponent<CharacterController>();
        rocketJump = GetComponent<RocketJump>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Setting the cursors lock state.
        Cursor.lockState = CursorLockMode.Locked;
        distanceToGround = controller.bounds.extents.y;
        //distanceToGround = capsuleCollider.bounds.extents.y;
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

    private void UpdateAnimations(bool grounded)
    {
        //Ground impact flag:
        if (grounded && justJumped)
        {
            mech_Animator.SetTrigger("GroundImpact");
            if(playerHandler.IsInvulnerable)
                sheild_Animator.SetTrigger("GroundImpact");
            justJumped = false;
        }

        // Grounded flag:
        mech_Animator.SetBool("Grounded", grounded);
        if(playerHandler.IsInvulnerable)
            sheild_Animator.SetBool("Grounded", grounded);

        // Walking flag:
        if(grounded)
        {
            mech_Animator.SetBool("Walk", (currentVelocity.magnitude > walkingVelocityPadding));
            if (playerHandler.IsInvulnerable)
                sheild_Animator.SetBool("Walk", (currentVelocity.magnitude > walkingVelocityPadding));

            float walkingSpeed = (currentVelocity.magnitude / timeToMaxWalkSpeed * walkSpeedMultiplier) * 0.1f;
            Debug.Log(walkingSpeed);

            mech_Animator.SetFloat("WalkSpeed", walkingSpeed);
        }
        else
            mech_Animator.SetBool("Walk", false);
    }

    private void FixedUpdate()
    {
        // Update the move vector.
        if(playerHandler.IsControllable)
            this.UpdateMoveVector();

        // Checking if the player is grounded:
        bool grounded = IsGrounded();
        playerHandler.IsGrounded = grounded;

        // Updating the animations:
        UpdateAnimations(grounded);

        // If the player is on the ground:
        if (grounded)
        {
            justBoosted = false;

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
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);

            // Checking for jump input.
            if (XCI.GetButtonDown(XboxButton.A, playerHandler.AssignedController) || Input.GetButtonDown("Jump") && playerHandler.IsControllable)
            {
                currentVelocity += Vector3.up * jumpHeight;
                if (!justJumped)
                {
                    mech_Animator.SetTrigger("Jump");

                    if (playerHandler.IsInvulnerable)
                        sheild_Animator.SetTrigger("Jump");

                    // Enabling just jumped flag:
                    justJumped = true;
                }
            }
        }
        else
        {
            // Calculating gravity.
            if (!playerHandler.Mech_RocketJump.IsBoosting)
            {
                // Added in a reset gravity bool to fix the stuttering jump pad issue:
                if (!shouldResetGravity)
                {
                    gravityVector = Vector3.down * gravity * Time.deltaTime;
                    currentVelocity += gravityVector;
                }
                else
                {
                    currentVelocity.y = 0.0f;
                    shouldResetGravity = false;
                }
            }

            // Calculating the in air movement.
            Vector3 inAirMoveVector = moveDirection * maxSpeed;
            inAirMoveVector -= currentVelocity;

            // Applying the air movement and gravity vectors.
            Vector3 velocityDiff = Vector3.ProjectOnPlane(inAirMoveVector, gravityVector);
            currentVelocity += velocityDiff * airAccelerationSpeed * Time.deltaTime;

            // Setting acceleration to be the velocity to prevent boosted movement on ground.
            acceleration = currentVelocity;

            // Check for jump boost
            if(!justBoosted)
            {
                if (XCI.GetButtonDown(XboxButton.A, playerHandler.AssignedController) || Input.GetButtonDown("Jump") && playerHandler.IsControllable)
                {
                    rocketJump.IsBoosting = true;
                    justBoosted = true;
                    --playerHandler.BoostPoints;
                }
            }
        }

        // Keeping track of the current velocity via the player handler.
        playerHandler.CurrentVelocity = currentVelocity;

        // Making the rigid bodies velocity equal the calculated velocity.
        controller.Move(currentVelocity * Time.deltaTime);
    }

    public Vector3 GetMoveVector()
    {
        return moveDirection;
    }

    public void ResetGravity()
    {
        shouldResetGravity = true;
    }

    private bool IsGrounded()
    {
        Vector3 startPos = controller.bounds.center;
        Vector3 endPos = new Vector3(controller.bounds.center.x, controller.bounds.min.y + 1.0f, controller.bounds.center.z);
        return Physics.CheckCapsule(startPos, endPos, 1.45f, this.gameObject.layer);
    }
}
