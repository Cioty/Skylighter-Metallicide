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

    private void Awake()
    {
        instance = this;
        //playerLoader = playerManager.GetComponent<LoadPlayers>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    // A function to handle resetting the occupied mech stations.
    public void ResetOccupiedMechStations()
    {
        // Looping through each station and setting the occupied status to false.
        foreach (Mech_Recovery station in mechRespawnStations)
        {
            if (station.IsOccupied)
                station.IsOccupied = false;
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
}
