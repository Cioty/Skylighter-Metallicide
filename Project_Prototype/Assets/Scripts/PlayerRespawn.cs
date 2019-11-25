/*=============================================================================
 * Game:        Metallicide
 * Version:     Alpha
 * 
 * Class:       PlayerRespawn.cs
 * Purpose:     Remembers the Mech Station that the Player is spawning at.
 * 
 * Author:      Nixon Sok
 * Team:        Skylighter
 * 
 * Deficiences: Does nothing for now
 * 
 *===========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    PlayerHandler playerHandler; // To get the Mech Station this player is spawning at, Health of both Core and Mech
    public RespawnArray gameManagerRespawn; // Get the respawn array from the Game Manager

    // Start is called before the first frame update
    void Start()
    {
        playerHandler = GetComponent<PlayerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
