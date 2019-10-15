using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    private int playerCount = 0;
    private Vector4[] onePlayer;
    private Vector4[] twoPlayer;
    private List<GameObject> activePlayers = new List<GameObject>();

    private void Awake()
    {
        onePlayer = new Vector4[1];
        twoPlayer = new Vector4[2];

        this.SetUpScreenSplit();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData.instance != null)
        {
            Vector3 position = Vector3.zero;
            playerCount = PlayerData.instance.AssignedPlayers;
            List<PlayerContainer> playerContainers = PlayerData.instance.GetTransferedPlayerContainers();

            for (int i = 0; i < playerCount; ++i)
            {
                GameObject player = Instantiate(playerPrefab, position, playerPrefab.transform.rotation, this.gameObject.transform);
                PlayerHandler handler = player.GetComponent<PlayerHandler>();
                handler.ID = i;
                player.tag = "Player";
                position.x += 20.0f;
                handler.AssignedController = playerContainers[i].Controller;
                activePlayers.Add(player);

                this.SetLayerRecursively(player, player.tag + i);
          
                // Hides the players mech from itself.
                handler.FirstPersonCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(player.tag + i));

                if (playerCount == 1 && i == 0)
                    continue;

                if (playerCount == 2 && i == 0)
                    handler.FirstPersonCamera.rect = new Rect(twoPlayer[0].x, twoPlayer[0].y, twoPlayer[0].z, twoPlayer[0].w);

                if (playerCount == 2 && i == 1)
                    handler.FirstPersonCamera.rect = new Rect(twoPlayer[1].x, twoPlayer[1].y, twoPlayer[1].z, twoPlayer[1].w);
            }

            // Destroys the kept data.
            Destroy(PlayerData.instance.gameObject);
        }
    }

    void SetLayerRecursively(GameObject gameObject, string gameTag)
    {
        gameObject.layer = LayerMask.NameToLayer(gameTag);
        foreach(Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, gameTag);
        }
    }

    void SetUpScreenSplit()
    {
        // Full screen.
        onePlayer[0] = new Vector4(0, 0, 1, 1);

        // Split screen.
        twoPlayer[0] = new Vector4(0, 0.5f, 1, 0.5f);
        twoPlayer[1] = new Vector4(0, 0, 1, 0.5f);
    }

    public List<GameObject> ActivePlayers
    {
        get { return activePlayers; }
        set { activePlayers = value; }
    }

}