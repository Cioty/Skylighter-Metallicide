using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayers : MonoBehaviour
{
    public static LoadPlayers instance;

    public bool debugMode = false;
    public GameObject playerPrefab;
    public RespawnArray respawnArray;
    private int playerCount = 0;

    private Vector4[] onePlayer;
    private Vector4[] twoPlayer;
    private Vector4[] threePlayer;
    private Vector4[] fourPlayer;
    private List<GameObject> activePlayers = new List<GameObject>();

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
        if (PlayerData.instance != null)
        {
            playerCount = PlayerData.instance.AssignedPlayers;
            List<PlayerContainer> playerContainers = PlayerData.instance.GetTransferedPlayerContainers();

            for (int i = 0; i < playerCount; ++i)
            {
                Transform randomSpawn = respawnArray.GetRandomSpawnPoint();
                GameObject player = Instantiate(playerPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
                PlayerHandler handler = player.GetComponent<PlayerHandler>();
                handler.MechTransform.position = randomSpawn.position;
                handler.MechTransform.rotation = randomSpawn.rotation;
                player.tag = "Player";
                handler.ID = i;
                handler.AssignedController = playerContainers[i].Controller;
                activePlayers.Add(player);

                // Hides the players mech from itself.
                this.SetLayerRecursively(player, player.tag + i, "HUD");
                handler.FirstPersonCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(player.tag + i));

                // If single player:
                if (playerCount == 1 && i == 0)
                    continue;

                // If more then one player:
                Vector4 screenPos = Vector4.zero;
                switch (playerCount)
                {
                    case 2:
                        screenPos = twoPlayer[i];
                        handler.FirstPersonCamera.rect = new Rect(screenPos.x, screenPos.y, screenPos.z, screenPos.w);
                        break;

                    case 3:
                        screenPos = threePlayer[i];
                        handler.FirstPersonCamera.rect = new Rect(screenPos.x, screenPos.y, screenPos.z, screenPos.w);
                        break;

                    case 4:
                        screenPos = fourPlayer[i];
                        handler.FirstPersonCamera.rect = new Rect(screenPos.x, screenPos.y, screenPos.z, screenPos.w);
                        break;
                }
            }
            // Destroys the transferd data.
            Destroy(PlayerData.instance.gameObject);
        }
        else if(debugMode)
        {
            /* temp hard code c/p, will clean this spawning code up along with this whole class soon*/
            Transform randomSpawn = respawnArray.GetRandomSpawnPoint();
            GameObject player = Instantiate(playerPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            PlayerHandler handler = player.GetComponent<PlayerHandler>();
            handler.MechTransform.position = randomSpawn.position;
            handler.MechTransform.rotation = randomSpawn.rotation;
            player.tag = "Player";
            handler.ID = 0;
            handler.AssignedController = 0;
            activePlayers.Add(player);

            // Hides the players mech from itself.
            this.SetLayerRecursively(player, player.tag + 0, "HUD");
            handler.FirstPersonCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(player.tag + 0));
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