using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeHandler : MonoBehaviour
{
    // List of all the players in the handler:
    public List<PlayerHandler> players = new List<PlayerHandler>();

    // Get the list of players:
    public List<PlayerHandler> GetPlayerList()
    {
        return players;
    }

    // Getter for the player handlers array:
    public PlayerHandler GetPlayerHandler(int index)
    {
        return players[index];
    }
}
