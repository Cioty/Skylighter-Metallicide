/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       LoadPlayers.cs *DEPRECATED*
 * Purpose:     Loads the players into the game either active with the contollers,
 *              or in debug mode. (All players movement disabled besides player0)

 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences: Re-worked this class into the PlayerManager.cs class for a
 *              preformance boost.
 * 
 *===========================================================================*/
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class LoadPlayers : MonoBehaviour
{
    // Singleton instance:
    public static LoadPlayers instance;

    // Scene object references:
    [Header("References")]
    public GameObject playerPrefab;
    public RespawnArray respawnArray;

    // Debug mode settings:
    [Header("Debug Mode")]
    public bool forceDebugMode = false;
    [Range(0, 4)]
    public int debugPlayerCount = 0;

    // Screen locations:
    private int playerCount = 0;
    private Vector4[] onePlayer;
    private Vector4[] twoPlayer;
    private Vector4[] threePlayer;
    private Vector4[] fourPlayer;

    // Player lists:
    private List<GameObject> activePlayers = new List<GameObject>();
    private List<PlayerContainer> playerContainers;

    private void Awake()
    {
        // Singleton instance.
        instance = this;

        // Singleplayer full screen.
        onePlayer = new Vector4[1];
        onePlayer[0] = new Vector4(0, 0, 1, 1);

        // Two player split screen.
        twoPlayer = new Vector4[2];
        twoPlayer[0] = new Vector4(0, 0.5f, 1, 0.5f);           // top half
        twoPlayer[1] = new Vector4(0, 0, 1, 0.5f);              // bot half

        // Three player split screen.
        threePlayer = new Vector4[3];
        threePlayer[0] = new Vector4(0, 0.5f, 1, 0.5f);         // top half
        threePlayer[1] = new Vector4(0, 0, 0.5f, 0.5f);         // bot left
        threePlayer[2] = new Vector4(0.5f, 0, 0.5f, 0.5f);      // bot right

        // Four player split screen.
        fourPlayer = new Vector4[4];
        fourPlayer[0] = new Vector4(0, 0.5f, 0.5f, 0.5f);       // top left
        fourPlayer[1] = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);    // top right
        fourPlayer[2] = new Vector4(0, 0, 0.5f, 0.5f);          // bot let
        fourPlayer[3] = new Vector4(0.5f, 0, 0.5f, 0.5f);       // bot right
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData.instance != null || forceDebugMode)
        {
            // Checking the StartInDebugMode flag in the PlayerData class.
            if (PlayerData.instance != null && PlayerData.instance.StartInDebugMode)
            {
                forceDebugMode = true;
                debugPlayerCount = PlayerData.instance.DebugPlayerCount;
            }

            // Getting the correct players, either from the assigned containers or the debug count:
            if(!forceDebugMode)
            {
                playerCount = PlayerData.instance.AssignedPlayers;
                playerContainers = PlayerData.instance.GetTransferedPlayerContainers();
            }
            else
                playerCount = debugPlayerCount;

            // Looping through each player and setting up the correct settings:
            for (int i = 0; i < playerCount; ++i)
            {
                // Getting random mech respawn station:
                Transform randomSpawn = respawnArray.GetRandomSpawnPoint();
                Vector3 fixedPosition = randomSpawn.position;
                fixedPosition.y += 0.5f;

                // Instantiating the player at that location and getting the playerHandler component from it:
                GameObject playerGO = Instantiate(playerPrefab, fixedPosition, randomSpawn.rotation, gameObject.transform);
                PlayerHandler playerHandler = playerGO.GetComponent<PlayerHandler>();

                // Assigning the player's ID:
                playerHandler.ID = i;

                // Assigning the correct controller:
                if(!forceDebugMode)
                    playerHandler.AssignedController = playerContainers[i].Controller;
                else
                    playerHandler.AssignedController = ((XboxController)i);

                // Adding the player to the active players list:
                activePlayers.Add(playerGO);

                // Assigns the correct view model layer:
                SetLayerRecursively(playerGO, playerGO.tag + i, "HUD");
                SetLayerRecursively(playerHandler.mechModelObject, playerGO.tag + i, "HUD");
                SetLayerRecursively(playerHandler.viewModelObject, playerGO.tag + i + "View");
                playerHandler.FirstPersonCamera.cullingMask |= 1 << LayerMask.NameToLayer(playerGO.tag + i + "View");
                playerHandler.FirstPersonCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(playerGO.tag + i));

                // Deactivating controls from all players other than player0:
                if(forceDebugMode)
                {
                    if (i > 0)
                        playerHandler.IsControllable = false;
                }

                // If single player:
                if (playerCount == 1 && i == 0)
                    continue;

                // If more then one player:
                Vector4 screenPos = Vector4.zero;
                switch (playerCount)
                {
                    case 2:
                        screenPos = twoPlayer[i];
                        playerHandler.FirstPersonCamera.rect = new Rect(screenPos.x, screenPos.y, screenPos.z, screenPos.w);
                        playerHandler.ThirdPersonCamera.rect = new Rect(screenPos.x, screenPos.y, screenPos.z, screenPos.w);
                        break;

                    case 3:
                        screenPos = threePlayer[i];
                        playerHandler.FirstPersonCamera.rect = new Rect(screenPos.x, screenPos.y, screenPos.z, screenPos.w);
                        playerHandler.ThirdPersonCamera.rect = new Rect(screenPos.x, screenPos.y, screenPos.z, screenPos.w);
                        break;

                    case 4:
                        screenPos = fourPlayer[i];
                        playerHandler.FirstPersonCamera.rect = new Rect(screenPos.x, screenPos.y, screenPos.z, screenPos.w);
                        playerHandler.ThirdPersonCamera.rect = new Rect(screenPos.x, screenPos.y, screenPos.z, screenPos.w);
                        break;
                }
            }
            
            // Destroys the transferd data.
            if (!forceDebugMode)
                Destroy(PlayerData.instance.gameObject);
        }
    }

    /*
     Setting the specified gameobject's layer recursively via string. String checking to skip a layer:
         */
    void SetLayerRecursively(GameObject gameObject, string layerName, string objectToSkip = "")
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
        foreach(Transform child in gameObject.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("PlayerMovement"))
                continue;

            if (child.gameObject.name == objectToSkip)
                continue;

            SetLayerRecursively(child.gameObject, layerName, objectToSkip);
        }
    }

    public List<GameObject> ActivePlayers
    {
        get { return activePlayers; }
        set { activePlayers = value; }
    }

}