using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public PTCAssigner ptcAssigner;
    private int assignedPlayers;
    private List<int> playerOrder = new List<int>();

    public static PlayerData instance;

    private void Awake()
    {
        instance = GetComponent<PlayerData>();

        // Making sure we don't destory the data on load of the game.
        DontDestroyOnLoad(this.gameObject);
    }

    public void Save()
    {
        List<PlayerContainer> playerContainers = ptcAssigner.GetPlayerContainers();
        for (int i = 0; i < assignedPlayers; ++i)
        {
            PlayerContainer container = playerContainers[i];
            PlayerOrder.Add(container.ID);
        }

        assignedPlayers = ptcAssigner.AssignedPlayers;
    }

    public List<int> PlayerOrder
    {
        get { return playerOrder; }
    }

    public int AssignedPlayers
    {
        get { return assignedPlayers; }
    }
}
