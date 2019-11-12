using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair_Kit : MonoBehaviour
{   
    private PlayerHandler playerHandler;
   
    LoadPlayers players;

    // The actual object this script affects
    private Renderer thisBox;
   
    bool isInteractable = true;

    public float coolDown = 5.0f;
    private float coolDownCounter;

    private float timerReset = 0.0f;

    private void Awake()
    {
        thisBox = GetComponent<Renderer>();
        coolDownCounter = timerReset;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject playerCollided = other.gameObject;

        if(playerCollided.gameObject.tag == "Player")
        {
            playerHandler = playerCollided.GetComponentInParent<PlayerHandler>();

            // If the player is a Mech
            if (playerHandler.CurrentState == StateManager.PLAYER_STATE.Mech)
            {             
                if (playerHandler.MechHealth >= playerHandler.MaxMechHealth)
                {
                    playerHandler.MechHealth = playerHandler.MaxMechHealth;
                }
                else
                {
                    playerHandler.MechHealth += 10;
                    isInteractable = false;
                }
                
            }

            // If the player is a core
            if (playerHandler.CurrentState == StateManager.PLAYER_STATE.Core)
            {             
                if (playerHandler.CoreHealth >= playerHandler.MaxCoreHealth)
                {
                    playerHandler.CoreHealth = playerHandler.MaxCoreHealth;
                }
                else
                {
                    playerHandler.CoreHealth += 10;
                    isInteractable = false;
                }
                
            }
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
            
            if (coolDownCounter >= coolDown)
            {
                isInteractable = true;               
                coolDownCounter = timerReset;              
            }
       }
       
       if (isInteractable)
       {
            thisBox.enabled = true;
       }
    }
}
