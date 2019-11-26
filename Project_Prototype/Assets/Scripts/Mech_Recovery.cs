/*=============================================================================
 * Game:        Metallicide
 * Version:     Alpha
 * 
 * Class:       Mech_Recovery.cs
 * Purpose:     Manages the respawing and recovery of the Mech player state.
 * 
 * Author:      Nixon Sok & Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences: Doesn't do animations just yet or activate the player's needed scripts
 *              You can still shoot and Dash
 * 
 *===========================================================================*/
using UnityEngine;
using UnityEngine.Animations;

public class Mech_Recovery : MonoBehaviour
{
    // Is the recovery station occupied?
    [SerializeField]
    private bool isOccupied = false;
    private bool isActive = true;
    //private bool mechElevate = false;
    private PlayerHandler playerHandlerStation = null;

    // A trigger to detect the ball collision.
    public Trigger respawnTrigger;

    // A trigger to detect a player ontop of the station.
    public Trigger occupiedTrigger;

    // A empty Game object's transform to spawn the player
    public Transform playerRefuelTransform;

    // Mech Station's animation
    // public Animation stationAnimation;
    public Animator animatorStation;

    //private Animator animatorStation;

    private void Start()
    {
        // Animation clips
        
    }

    private void Update()
    {
        // Checking for core collision:
        GameObject collidedObject = respawnTrigger.CollidedGameObject();
        if (collidedObject && collidedObject.tag == "Player")
        {
            // Getting the player's handler from the collided game object.
            PlayerHandler playerHandler = collidedObject.GetComponentInParent<PlayerHandler>();
            if (respawnTrigger.IsEnabled() && playerHandler && playerHandler.CurrentState == StateManager.PLAYER_STATE.Core)
            {
                // Adding a point for respawning.
                playerHandler.PlayerStats.HasEscaped();

                // Spawning the player at the station location.
                SpawnPlayer(playerHandler);
            }
        }

        // Check to see if respawn is in progress
        if (playerHandlerStation != null)
        {
            // Is it respawning?
            if (playerHandlerStation.IsSpawning)
            {
                MechStationAnimation(playerHandlerStation);
            }
        }

        // Checking if a player is ontop of the station:
        GameObject occupied = occupiedTrigger.CollidedGameObject();

        // Setting the occupied flag based on the trigger state:
        this.isOccupied = (occupiedTrigger.IsEnabled()) ? true : false;
    }

    // A function to spawn a player at this stations location.

        /* UPDATES */
        // Player's are set to Mech first
        // All player's movement and weapons are disabled(not yet finished)
        // The animation will be done via another function
    public void SpawnPlayer(PlayerHandler playerHandler)
    {
        // Remember which player is spawning here
        playerHandlerStation = playerHandler;

        // Setting the player's state to the mech state.
        playerHandlerStation.StateManager.SetState(StateManager.PLAYER_STATE.Mech);

        // Resetting the player's stats.
        playerHandlerStation.MechHealth = playerHandler.MaxMechHealth;
        playerHandlerStation.CoreHealth = playerHandler.MaxCoreHealth;

        playerHandlerStation.MechCharacterController.enabled = false;
        playerHandlerStation.IsControllable = false;

        // Prevent Player from looking around the Mech Station
        playerHandlerStation.transform.forward = playerRefuelTransform.forward;
        playerHandlerStation.MechCamera.transform.forward = playerRefuelTransform.forward;
        //wwplayerHandler.mechObject;
        playerHandlerStation.MechCamera.enabled = false;

        // Testing spawning player at the new spawn point
        playerHandlerStation.mechObject.transform.position = playerRefuelTransform.TransformPoint(playerRefuelTransform.localPosition);
        playerHandlerStation.mechObject.transform.forward = playerRefuelTransform.forward;

        playerHandlerStation.IsSpawning = true;
        playerHandler.IsAlive = true;
    }

    // When SpawnPlayer has ended, this plays
    public void MechStationAnimation(PlayerHandler playerHandler)
    {
        if(playerHandlerStation.IsControllable)
            playerHandlerStation.IsControllable = false;

        // Close Hatch, regardless if the ball has entered
        if (!animatorStation.GetBool("Close_Hatch"))
            animatorStation.SetBool("Close_Hatch", playerHandler.IsSpawning);

        // When the Hatch is closed
        if (animatorStation.GetBool("Close_Hatch"))
        {
            // Start the animation.
            animatorStation.SetTrigger("Respawn");
        }

        // When it gets to Elevate
        if (animatorStation.GetCurrentAnimatorStateInfo(0).IsName("Elevate"))
        {
            // Set Player's position to where ever the elevator's spawn point is now
            playerHandler.mechObject.transform.position = (playerRefuelTransform.TransformPoint(playerRefuelTransform.localPosition) + new Vector3(0, 1f, 0));
        }

        // Now when it's now elevated, the barrier goes down. Give control back to the player
        if (animatorStation.GetCurrentAnimatorStateInfo(0).IsName("Barrier Down"))
        {
            playerHandler.MechCharacterController.enabled = true;
            playerHandler.MechCamera.enabled = true;
            playerHandler.IsSpawning = false;
            playerHandler.IsControllable = true;
            playerHandlerStation = null;
            animatorStation.ResetTrigger("Barrier_Down");
            animatorStation.ResetTrigger("Respawn");
            animatorStation.SetBool("Close_Hatch", false);
        }
    }

    public void MechElevatorMove(int start)
    {
        if(start == 1)
        {
            //Start setting position


        } else
        {
            //Stop setting position

        }
    }

    // A property for the occupied boolean.
    public bool IsOccupied
    {
        set { isOccupied = value;}
        get { return isOccupied; }
    }

    // A property for the active boolean.
    public bool IsActive
    {
        set { isActive = value;}
        get { return isActive; }
    }
   
}
