using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechManager : MonoBehaviour
{
    private PlayerHandler playerHandler;
    private StateManager stateManager;

    private void Awake()
    {
        playerHandler = GetComponentInParent<PlayerHandler>();
        stateManager = GetComponentInParent<StateManager>();
    }

    private void Update()
    {
        if(playerHandler.mechHealth <= 0)
        {
            // death screen
        }
    }
}
