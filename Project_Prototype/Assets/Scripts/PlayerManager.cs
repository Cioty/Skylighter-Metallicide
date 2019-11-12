/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       PlayerManager.cs
 * Purpose:     Manages the loading of players into the game scene.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerManager : MonoBehaviour
{
    // Singleton instance:
    public static PlayerManager instance;

    [Header("References")]
    public GameObject playerPrefab;
    public List<GamemodeHandler> splitScreenModeArray = new List<GamemodeHandler>();
    public RespawnArray respawnArray;

    [Header("Debug Options")]
    public bool forceDebugMode = false;
    [Range(1, 4)]
    public int debugPlayerCount = 1;

    // Private variables:
    private PlayerData.SplitScreenMode splitScreenMode;
    private GamemodeHandler currentGameMode;
    private List<PlayerHandler> activePlayers = new List<PlayerHandler>();
    private List<PlayerContainer> playerContainers;
    private int playerCount = 0;
    private bool containersValid = false;

    private void Awake()
    {
        instance = this;

        if(PlayerData.instance != null && PlayerData.instance.StartInDebugMode)
        {
            forceDebugMode = true;
            debugPlayerCount = PlayerData.instance.DebugPlayerCount;
        }

        if (PlayerData.instance != null && !forceDebugMode)
        {
            splitScreenMode = PlayerData.instance.CurrentSplitScreenMode;
            playerContainers = PlayerData.instance.GetTransferedPlayerContainers();
            containersValid = true;
        }

        if(forceDebugMode)
        {
            PlayerData.SplitScreenMode splitDebugMode = ((PlayerData.SplitScreenMode)(debugPlayerCount - 1));
            splitScreenMode = splitDebugMode;
        }
    }

    private void Start()
    {
        ActivateCorrectScreenView();
        ActivateAndPositionAllPlayers();
        
        // Resets the occupancy of the mech station:
        RespawnArray.instance.ResetMechRespawnStations = true;

        // Destroys the transferd data.
        if (!forceDebugMode && PlayerData.instance != null)
            Destroy(PlayerData.instance.gameObject);
    }
    
    private void ActivateCorrectScreenView()
    {
        currentGameMode = splitScreenModeArray[(int)splitScreenMode];
        activePlayers = currentGameMode.GetPlayerList();

        // Checking if debugMode:
        if (forceDebugMode)
            playerCount = debugPlayerCount;
        else
        {
            playerCount = activePlayers.Count;
        }

        Debug.Log("Setting gamestate up for " + playerCount + " players!");
        currentGameMode.gameObject.SetActive(true);
    }

    private void ActivateAndPositionAllPlayers()
    {
        for(int i = 0; i < playerCount; ++i)
        {
            // Getting the playerHandler for ease of use:
            PlayerHandler playerHandler = activePlayers[i];

            // Activating the player:
            playerHandler.gameObject.SetActive(true);

            //Assigning the correct controller:
            if (!forceDebugMode)
            {
                // Checking if the players have been loaded correctly:
                if (containersValid)
                {
                    // Assigning the players controller to the controller saved in the playerContainer: 
                    playerHandler.AssignedController = playerContainers[i].Controller;
                }
                else
                {
                    // Logging an error to the user to let them know the player hasn't loaded properly:
                    Debug.LogError("Player container " + "'" + i + "'" + " is null! " +
                        "Make sure you're loading the player from the Player-Manager and one doesn't already exist in the scene.");
                }
            }
            else
                // Converting index 'i' into an xbox controller, then setting it to the indexed player handler:
                playerHandler.AssignedController = ((XboxController)i);

            //Deactivating controls from all players other than player0:
            if (forceDebugMode)
            {
                // If not player0
                if (i > 0)
                    playerHandler.IsControllable = false;
            }

            // Spawning the player in.
            playerHandler.RandomSpawn_Unactive();
        }
    }

    public List<PlayerHandler> ActivePlayers
    {
        get { return activePlayers; }
    }
}
