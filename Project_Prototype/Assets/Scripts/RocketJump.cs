using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class RocketJump : MonoBehaviour
{
    // Neccessary things needed from the player
    private Rigidbody rb;
    private Vector3 boostDirection;
    private PlayerHandler playerHandler;
    private Dashing mechDash;

    private Vector3 currentVelocity;

    // The two variables that affect the speed and duration
    public float boostJumpSpeed = 5.0f;
    public float boostJumpDuration = 5.0f;

    private float boostTimer;
    private float boostJumpEnd = 0.0f;

    private bool isBoostJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boostDirection = transform.up;
        playerHandler = GetComponentInParent<PlayerHandler>();
        mechDash = GetComponent<Dashing>();

    }

    private void Start()
    {
        boostTimer = boostJumpDuration;      
    }

    void Update()
    {
        rocketBoost();
    }

    void rocketBoost()
    {       
        if (isBoostJumping)
        {
            if (mechDash.IsDashing)
                mechDash.IsDashing = false;


            if (boostTimer > boostJumpEnd)
            {
                playerHandler.MechImpactRecevier.AddImpact(boostDirection, boostJumpSpeed);               
                boostTimer -= 1 * Time.deltaTime;
            }

            if (boostTimer <= boostJumpEnd)
            {
                boostTimer = boostJumpDuration;
                isBoostJumping = false;
            }
        }
    }

    public bool IsBoosting
    {
        get { return isBoostJumping; }
        set { isBoostJumping = value; }
    }
}
