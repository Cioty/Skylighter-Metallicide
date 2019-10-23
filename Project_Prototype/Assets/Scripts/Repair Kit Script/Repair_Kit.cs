using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair_Kit : MonoBehaviour
{   
    public Transform platform;

    [SerializeField] private GameObject Mech;
    [SerializeField] private GameObject Ball;

    private PlayerHandler playerHandler;
    private StateManager playerState;

    LoadPlayers players;

    // The actual object this script affects
    private Renderer thisBox;

    bool isInteractable = true;

    public float coolDown;
    private float coolDownCounter;

    private float timerReset = 0.0f;

    private void Awake()
    {
        thisBox = GetComponent<Renderer>();

        platform = GetComponent<Transform>();

        transform.localPosition = platform.localPosition;
    
        coolDownCounter = timerReset;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject playerCollided = other.gameObject;

        playerHandler = playerCollided.GetComponent<PlayerHandler>();
        playerState = playerCollided.GetComponent<StateManager>();


        // If the player is a Mech
       if (playerState.CurrentState == StateManager.PLAYER_STATE.Mech)
       {
            playerHandler.MechHealth += 10;
            if (playerHandler.MechHealth > playerHandler.MaxMechHealth)
            {
                playerHandler.MechHealth = playerHandler.MaxMechHealth;
            }
            isInteractable = false;
       }

       // If the player is a core
       if (playerState.CurrentState == StateManager.PLAYER_STATE.Core)
       {
           playerHandler.CoreHealth += 10;
           if (playerHandler.CoreHealth > playerHandler.MaxCoreHealth)
           {
               playerHandler.CoreHealth = playerHandler.MaxCoreHealth;
           }
           isInteractable = false;
       }
    }

    // Update is called once per frame
    void Update()
    {
       if (!isInteractable)
       {
            if (thisBox.enabled)
                thisBox.enabled = false;


            coolDownCounter += Time.deltaTime;
            //Debug.Log("Cooldown is: " + coolDownCounter.ToString());
            if (coolDownCounter >= coolDown)
            {
                isInteractable = true;
                //Debug.Log("Repair kit can be used again: " + isInteractable.ToString());
                coolDownCounter = timerReset;
                //Debug.Log("Cooldown is reset: " + coolDownCounter.ToString());
            }
       }
       
       if (isInteractable)
       {
            thisBox.enabled = true;
       }
    }
}
