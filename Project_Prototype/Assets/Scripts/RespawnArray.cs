using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnArray : MonoBehaviour
{
    //public GameObject playerObject;
    public GameObject playerManager;
    private LoadPlayers playerLoader;

    // This will be used for onTriggerCollisions
    private StateManager playerState;

    // Respawn timer
    public float deathTimer = 3;

    // For the Ball/Player dies
    private PlayerHandler playerHandler;

    // Keeps track of all the spawn points
    public List<GameObject> respawnPoints = new List<GameObject>();

    // Random Number to pick a random spawn point
    int randNumber;

    private void Awake()
    {
        playerLoader = playerManager.GetComponent<LoadPlayers>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    // Update is called once per frame
    void Update()
    {
        MechRespawn();
    }

    void MechRespawn()
    {
        foreach(GameObject player in playerLoader.ActivePlayers)
        {
            playerHandler = player.GetComponent<PlayerHandler>();


            if (playerHandler.mechHealth <= 0)
            {
                if (playerHandler.mechHealth < 0)
                    playerHandler.mechHealth = 0;

                if (deathTimer <= 0)
                {
                    Spawn(player);
                }
                else if (deathTimer > 0)
                {
                    deathTimer -= 1 * Time.deltaTime;
                    Debug.Log("Time til respawn " + deathTimer.ToString());
                }
            }
        }
    }

    // The batch of code that handles respawning players
    void Spawn(GameObject playerObject)
    {
        Debug.Log("Start");
        randNumber = Random.Range(0, respawnPoints.Count);
        playerHandler.CurrentState = StateManager.PLAYER_STATE.Mech;

        Vector3 random = respawnPoints[randNumber].transform.position;
        random.y += 2.5f;

        playerHandler.mechObject.transform.position = random;
        playerHandler.mechObject.transform.rotation = respawnPoints[randNumber].transform.rotation;
        playerHandler.MechHealth = playerHandler.MaxCoreHealth;
        deathTimer = 3;
        Debug.Log("End");
    }

    public Transform GetRandomSpawnPoint()
    {
        randNumber = Random.Range(0, respawnPoints.Count);
        return respawnPoints[randNumber].transform;
    }
}
