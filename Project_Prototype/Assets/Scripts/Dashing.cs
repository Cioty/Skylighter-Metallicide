using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    private MovementController movementDir;
    private Rigidbody rb;

    [Header("Dashing")]
    // Dashing stuff
    public AnimationCurve speedGraph = new AnimationCurve();
   
    public float speed = 2.0f;
    
    //private float playerDashSpeed;
    //private static float initialSpeedDash = 0.0f;

    [Header("Dash Input")]
    public KeyCode dashKey;

    // Trigger, Direction & counter
    private bool dashTrig;

    // Last direction that constantly obtains Mech/Player direction
    private Vector3 lastDir;

    // Amount of times you can boost
    private int boostPoints = 3;

    // Dashing duration
    public float duration = 3.0f;
    private float thrusterTimer;

    // The things that will be used to regenerate boost uses
    public float boostRegen = 2.0f;
    private float boostTime;

    // Almighty Zero!!!
    private float zero = 0.0f;
  
    private void Awake()
    {
        // Get the rigidbody of the Mech, or whatever
        rb = GetComponent<Rigidbody>();

        thrusterTimer = zero;                        
    }

    // Dashing function
    private void isDashing()
    {
        float horizontal_move = Input.GetAxis("Horizontal");
        float vertical_move = Input.GetAxis("Vertical");

        // When dashKey press, temporarily stop movement (No falling, no nothing)
        if (Input.GetKeyDown(dashKey) && boostPoints > 0)
        {
            // To get the Mech's last direction
            lastDir = transform.forward.normalized * vertical_move + transform.right.normalized * horizontal_move;

            thrusterTimer = zero;
          
            dashTrig = true;
            
            --boostPoints;
            Debug.Log(boostPoints);
        }

        if (dashTrig)
        {           
            if (thrusterTimer < duration)
            {
                thrusterTimer += Time.deltaTime;
                rb.velocity = lastDir * speedGraph.Evaluate(thrusterTimer / duration) * speed;                
            }

            if (thrusterTimer >= duration)
            {  
                dashTrig = false;
                thrusterTimer = zero;
            }
        }
                  
        if (boostPoints < 3)
        {
            if (boostTime < boostRegen)
            {
                boostTime += 1.0f * Time.deltaTime;
            }

            if (boostTime > boostRegen)
            {
                boostTime = zero;
                boostPoints++;
                Debug.Log(boostPoints);
            }
        }
    }

    private void Update()
    {
        isDashing();
    }  
}
