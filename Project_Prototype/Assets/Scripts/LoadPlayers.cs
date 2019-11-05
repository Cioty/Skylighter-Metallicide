using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class LoadPlayers : MonoBehaviour
{
    public static LoadPlayers instance;

    public bool debugMode = false;
    public int debugPlayerCount = 1;
    public GameObject playerPrefab;
    public RespawnArray respawnArray;
    private int playerCount = 0;

    private Vector4[] onePlayer;
    private Vector4[] twoPlayer;
    private Vector4[] threePlayer;
    private Vector4[] fourPlayer;
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
        if (PlayerData.instance != null || debugMode)
        {
            if (PlayerData.instance.isDebugMode)
            {
                debugMode = true;
                debugPlayerCount = PlayerData.instance.debugPlayerCount;
            }

            if(!debugMode)
            {
                playerCount = PlayerData.instance.AssignedPlayers;
                playerContainers = PlayerData.instance.GetTransferedPlayerContainers();
            }
            else
                playerCount = debugPlayerCount;

            for (int i = 0; i < playerCount; ++i)
            {
                Transform randomSpawn = respawnArray.GetRandomSpawnPoint();
                Vector3 fixedPosition = randomSpawn.position;
                fixedPosition.y += 0.5f;
                GameObject playerGO = Instantiate(playerPrefab, fixedPosition, randomSpawn.rotation, this.gameObject.transform);
                PlayerHandler playerHandler = playerGO.GetComponent<PlayerHandler>();

                // Assigning the player's ID.
                playerHandler.ID = i;

                // Assigning the correct controller.
                if(!debugMode)
                    playerHandler.AssignedController = playerContainers[i].Controller;
                else
                    playerHandler.AssignedController = ((XboxController)i);

                // Adding the player to the active players list.
                activePlayers.Add(playerGO);

                // Assigns the correct view model layer.
                this.SetLayerRecursively(playerGO, playerGO.tag + i, "HUD");
                this.SetLayerRecursively(playerHandler.mechModelObject, playerGO.tag + i, "HUD");
                this.SetLayerRecursively(playerHandler.viewModelObject, playerGO.tag + i + "View");
                playerHandler.FirstPersonCamera.cullingMask |= 1 << LayerMask.NameToLayer(playerGO.tag + i + "View");
                playerHandler.FirstPersonCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(playerGO.tag + i));

                // Deactivating controls from other players:
                if(debugMode)
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
            if (!debugMode)
                Destroy(PlayerData.instance.gameObject);
        }
    }

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