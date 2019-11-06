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
    public List<Mech_Recovery> mechRespawnStations = new List<Mech_Recovery>();

    // Random Number to pick a random spawn point
    int randNumber = 0;

    // A bool to determine if we want to reset the mech stations or not.
    private bool shouldResetMechStations = false;

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
        ResetOccupiedMechStations();
    }

    // A function to handle resetting the occupied mech stations.
    void ResetOccupiedMechStations()
    {
        if (shouldResetMechStations)
        {
            // Looping through each station and setting the occupied status to false.
            foreach (Mech_Recovery station in mechRespawnStations)
            {
                station.IsOccupied = false;
            }
            
            // Turning off the should resetMechStations flag.
            shouldResetMechStations = false;
        }
    }

    // A function to handle respawning the player in their mech.
    void MechRespawn()
    {
        foreach (GameObject player in playerLoader.ActivePlayers)
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
        randNumber = Random.Range(0, mechRespawnStations.Count);
        playerHandler.CurrentState = StateManager.PLAYER_STATE.Mech;

        Vector3 random = mechRespawnStations[randNumber].transform.position;
        random.y += 2.5f;

        playerHandler.mechObject.transform.position = random;
        playerHandler.mechObject.transform.rotation = mechRespawnStations[randNumber].transform.rotation;
        playerHandler.MechHealth = playerHandler.MaxMechHealth;
        deathTimer = 3;
        Debug.Log("End");
    }

    // A function to get a random spawn point.
    public Transform GetRandomSpawnPoint()
    {
        // Getting random mech station.
        randNumber = Random.Range(0, mechRespawnStations.Count);
        Mech_Recovery randomSpawnPoint = mechRespawnStations[randNumber];

        // Checking if it is occupied.
        if (!randomSpawnPoint.IsOccupied)
        {
            randomSpawnPoint.IsOccupied = true;
            return randomSpawnPoint.transform;
        }

        // Recursively searching for an avaliable mech station.
        return GetRandomSpawnPoint();
    }

    // A property for the reset mech repsawn stations flag.
    public bool ResetMechRespawnStations
    {
        get { return shouldResetMechStations; }
        set { shouldResetMechStations = value; }
    }
}
