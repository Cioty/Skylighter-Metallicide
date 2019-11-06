using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Dashing : MonoBehaviour
{
    //public GameObject playerObject;
    public GameObject mechObject;
    private CharacterController characterController;
    //private Rigidbody rb;
    private MechController mechController;
    private PlayerHandler playerHandler;

    private RocketJump rocketJump;

    //private MovementController movementDir;

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
        playerHandler = mechObject.GetComponentInParent<PlayerHandler>();
        mechController = playerHandler.MechController;
        thrusterTimer = zero;        
        rocketJump = GetComponent<RocketJump>();
    }

    // Dashing function
    private void isDashing()
    {
        float horizontal_move = Input.GetAxis("Horizontal");
        float vertical_move = Input.GetAxis("Vertical");

        float leftTrigHeight = XCI.GetAxis(XboxAxis.LeftTrigger, playerHandler.AssignedController);
        // When dashKey press, temporarily stop movement (No falling, no nothing)
        if ((Input.GetKeyDown(dashKey) || XCI.GetButtonDown(XboxButton.LeftBumper, playerHandler.AssignedController) || leftTrigHeight >= 0.5f) && boostPoints > 0)
        {
            // To get the Mech's last direction
            lastDir = transform.forward.normalized * vertical_move + transform.right.normalized * horizontal_move;

            thrusterTimer = zero;
          
            dashTrig = true;
            rocketJump.IsBoosting = false;

            --playerHandler.BoostPoints;
        }

        if (dashTrig)
        {           
            if (thrusterTimer < duration)
            {
                thrusterTimer += Time.deltaTime;

                playerHandler.MechImpactRecevier.AddImpact(lastDir * speedGraph.Evaluate(thrusterTimer / duration), speed);
                //rb.AddForce(lastDir * speedGraph.Evaluate(thrusterTimer / duration) * speed, ForceMode.Impulse);
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
            }
        }
    }

    private void Update()
    {
        isDashing();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       // Debug.Log("Hit " + hit.gameObject.name);
    }

    public bool IsDashing
    { 
        get { return dashTrig; }
        set { dashTrig = value; }
    }
}
