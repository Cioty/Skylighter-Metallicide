using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnArray : MonoBehaviour
{
    public static RespawnArray instance;

    //public GameObject playerObject;
    public GameObject playerManager;
    public GameManager gameManager;
    private LoadPlayers playerLoader;

    // This will be used for onTriggerCollisions
    private StateManager playerState;

    // Respawn timer
    public float deathTimer = 3;

    // Keeps track of all the spawn points
    public List<Mech_Recovery> mechRespawnStations = new List<Mech_Recovery>();

    // Random Number to pick a random spawn point
    int randNumber = 0;

    // A bool to determine if we want to reset the mech stations or not.
    private bool shouldResetMechStations = false;

    private void Awake()
    {
        instance = this;
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
