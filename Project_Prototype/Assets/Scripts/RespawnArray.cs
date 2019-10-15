using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnArray : MonoBehaviour
{
    public GameObject playerObject;



    // The Player in Ball state
    public GameObject inGameBall;
    public GameObject inGameMech;
    private GameObject ball;

    // This will be used for onTriggerCollisions
    private StateManager playerState;

    // Respawn timer
    public float deathTimer = 3;

    // For the Ball/Player dies
    private PlayerHandler ballHandler;
    private PlayerHandler mechHandler;
    private PlayerHandler playerHandler;

    // Character controller
    CharacterController enabler;

    // Keeps track of all the spawn points
    public List<GameObject> respawnPoints = new List<GameObject>();

    // Random Number to pick a random spawn point
    int randNumber;

    private void Awake()
    {

        playerState = inGameBall.GetComponentInParent<StateManager>();
        
        playerHandler = playerObject.GetComponent<PlayerHandler>();
        
        enabler = inGameMech.GetComponentInParent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        // Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        MechRespawn();
    }

    void MechRespawn()
    {
        // If Mech's health is below zero
        //if (mechHandler.Health < 0.0f)
        //{
        //    mechHandler.Health = 0;
        //    enabler.enabled = false;
        //    //deathScreenEnabler.enabled = true;
        //    // randNumber = Random.Range(0, respawnPoints.Count);
        //}

        if (playerHandler.mechHealth <= 0)
        {
            if (enabler.enabled)
            {
                enabler.enabled = false;
            }

            if(playerHandler.mechHealth < 0)
                playerHandler.mechHealth = 0;
            
            
            if (deathTimer <= 0)
            {
                Spawn();
            }
            else if (deathTimer > 0)
            {
                deathTimer -= 1 * Time.deltaTime;
                Debug.Log("Time til respawn " + deathTimer.ToString());
            }
        }
    }

    void Spawn()
    {
        enabler.enabled = false;
        Debug.Log("Start");
        randNumber = Random.Range(0, respawnPoints.Count);

        playerObject.transform.position = respawnPoints[randNumber].transform.position;
        enabler.enabled = true;
        playerHandler.mechHealth = 2;
        deathTimer = 3;
        Debug.Log("End");
    }

    public Transform GetRandomSpawnPoint()
    {
        randNumber = Random.Range(0, respawnPoints.Count);
        return respawnPoints[randNumber].transform;
    }
}
