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
 * Deficiences:
 * 
 *===========================================================================*/
using UnityEngine;

public class Mech_Recovery : MonoBehaviour
{
    // Is the recovery station occupied?
    private bool isOccupied = false;
    private bool isActive = true;

    // A trigger to detect the ball collision.
    public Trigger respawnTrigger;

    private void Update()
    {
        GameObject collidedObject = respawnTrigger.CollidedGameObject();
        if (collidedObject && collidedObject.tag == "Player")
        {
            // Getting the player's handler from the collided game object.
            PlayerHandler playerHandler = collidedObject.GetComponentInParent<PlayerHandler>();
            if (respawnTrigger.IsEnabled() && playerHandler.CurrentState == StateManager.PLAYER_STATE.Core)
            {
                // Adding a point for respawning.
                playerHandler.PlayerStats.HasEscaped();

                // Spawning the player at the station location.
                SpawnPlayer(playerHandler);
            }
        }
    }

    // A function to spawn a player at this stations location.
    public void SpawnPlayer(PlayerHandler playerHandler)
    {
        // Resetting the player's stats.
        playerHandler.MechHealth = playerHandler.MaxMechHealth;
        playerHandler.CoreHealth = playerHandler.MaxCoreHealth;
        playerHandler.MechCharacterController.enabled = false;
        playerHandler.mechObject.transform.position = this.transform.position;
        playerHandler.mechObject.transform.rotation = this.transform.rotation;
        playerHandler.MechCharacterController.enabled = true;
        playerHandler.IsAlive = true;

        // Setting the player's state to the mech state.
        playerHandler.StateManager.SetState(StateManager.PLAYER_STATE.Mech);
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
