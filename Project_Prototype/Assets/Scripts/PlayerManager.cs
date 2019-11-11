using System.Collections;
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
    private List<PlayerHandler> activePlayers;
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
                if (containersValid)
                {
                    playerHandler.AssignedController = playerContainers[i].Controller;
                }
                else
                {
                    playerHandler.AssignedController = ((XboxController)i);
                    Debug.LogWarning("Containers are not valid! Assigning xbox controller and forcing a player to spawn!");
                    Debug.LogError("Player container " + "'" + i + "'" + " is null! " +
                        "Make sure you're loading the player from the Player-Manager and one doesn't already exist in the scene.");
                }
            }
            else
                playerHandler.AssignedController = ((XboxController)i);

            //Deactivating controls from all players other than player0:
            if (forceDebugMode)
            {
                if (i > 0)
                    playerHandler.IsControllable = false;
            }

            // Adding the player to the active players list:
            activePlayers.Add(playerHandler);

            // Spawning the player in.
            playerHandler.SpawnAsUnactive();
        }
    }
}
