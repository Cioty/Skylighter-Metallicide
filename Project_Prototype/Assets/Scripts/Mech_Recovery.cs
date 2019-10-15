/*=============================================================================
 * Game:        Metallicide
 * Version:     Alpha
 * 
 * Class:       Mech_Recovery.cs
 * Purpose:     Manages the respawing and recovery of the Mech player state.
 * 
 * Author:      Nixon Sok
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_Recovery : MonoBehaviour
{
    // The Player in Ball state
    public GameObject inGameBall;
    public GameObject inGameMech;
    private GameObject ball;

    // This will be used for onTriggerCollisions
    private StateManager playerState;

    // Respawn timer
    public float deathTimer = 5;

    // For the Ball/Player dies
    private PlayerHandler ballHandler;
    private PlayerHandler mechHandler;

    // Character controller
    CharacterController enabler;

    // Death Screen
    // public Canvas deathScreenEnabler;

    private void Awake()
    {
        
        playerState = inGameBall.GetComponentInParent<StateManager>();
        ballHandler = inGameBall.GetComponent<PlayerHandler>();
        mechHandler = inGameMech.GetComponent<PlayerHandler>();

        enabler = inGameMech.GetComponentInParent<CharacterController>();
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        ball = collision.gameObject;

        //if (ball == inGameBall)
        //{
        //    playerState.SetState(StateManager.PLAYER_STATE.Mech);
        //}
        //else if (ballHandler.Health <= 0)
        //{
        //    playerState.SetState(StateManager.PLAYER_STATE.Mech);
        //    inGameMech.transform.position = transform.position;
        //    ballHandler.Health = 10;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // MechRespawn();
    }

    void MechRespawn()
    {
        // If Mech's health is below zero
        if (mechHandler.mechHealth <= 0.0f)
        {
            mechHandler.mechHealth = 0;
            enabler.enabled = false;
            //deathScreenEnabler.enabled = true;
        }

        if (mechHandler.mechHealth == 0)
        {
            if (deathTimer <= 0)
            {
                inGameMech.transform.parent.position = transform.position;
                enabler.enabled = true;
                mechHandler.mechHealth = 2;
                deathTimer = 5;
            }
            else if (deathTimer > 0)
            {
                deathTimer -= 1 * Time.deltaTime;
                Debug.Log("Time til respawn " + deathTimer.ToString());
            }
        }
    }
}
