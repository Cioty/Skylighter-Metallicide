using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Dashing : MonoBehaviour
{
    //public GameObject playerObject;
    public GameObject mechObject;

    public List<ParticleSystem> boosters = new List<ParticleSystem>();

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

    // Dashing duration
    public float duration = 3.0f;
    private float thrusterTimer;

    // Almighty Zero!!!
    private float zero = 0.0f;

    // Ramming:
    [Header("Ramming")]
    public float ramForce = 10.0f;
    public int ramDamage = 10;
    private bool hasRammedThisDash = false;
  
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
        //float horizontal_move = Input.GetAxis("Horizontal");
        //float vertical_move = Input.GetAxis("Vertical");

        float leftTrigHeight = XCI.GetAxis(XboxAxis.LeftTrigger, playerHandler.AssignedController);
        // When dashKey press, temporarily stop movement (No falling, no nothing)
        if ((Input.GetKeyDown(dashKey) || XCI.GetButtonDown(XboxButton.LeftBumper, playerHandler.AssignedController) || leftTrigHeight >= 0.5f) && playerHandler.BoostPoints > 0)
        {
            // To get the Mech's last direction
            // lastDir = transform.forward.normalized * vertical_move + transform.right.normalized * horizontal_move;
            lastDir = playerHandler.MechController.GetMoveVector();
            if (lastDir == Vector3.zero)
                lastDir = this.transform.forward;

            thrusterTimer = zero;
          
            dashTrig = true;
            rocketJump.IsBoosting = false;

            --playerHandler.BoostPoints;
        }

        if (dashTrig)
        {
            foreach(ParticleSystem booster in boosters)
                booster.Play();

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
                hasRammedThisDash = false;
                foreach (ParticleSystem booster in boosters)
                    booster.Stop();
            }
        }
                  
    }

    private void Update()
    {
        isDashing();
    }

    /* Ramming function: */
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // If the collided object is a player:
        if (!hasRammedThisDash && hit.gameObject.tag == "Player")
        {
            // Grab the playerHandler component from the parent object:
            PlayerHandler hit_playerHandler = hit.gameObject.GetComponentInParent<PlayerHandler>();

            // Check if the player is in their mech state:
            if(hit_playerHandler.IsInMech())
            {
                // Adding the impact to the mech:
                hit_playerHandler.MechImpactRecevier.AddImpact(lastDir, ramForce);

                // Making the mech take damage:
                hit_playerHandler.Mech_TakeDamage(ramDamage);

                // Enabling the just rammed flag to prevent constant hits:
                hasRammedThisDash = true;
            }
        }
    }

    public bool IsDashing
    { 
        get { return dashTrig; }
        set { dashTrig = value; }
    }
}
