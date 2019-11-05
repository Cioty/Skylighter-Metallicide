using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public PTCAssigner ptcAssigner;
    private int assignedPlayers;
    private List<PlayerContainer> transferedPlayerContainers = new List<PlayerContainer>();
    public static PlayerData instance;
    public bool isDebugMode = false;
    public int debugPlayerCount = 2;

    private void Awake()
    {
        instance = GetComponent<PlayerData>();

        // Making sure we don't destory the data on load of the game.
        DontDestroyOnLoad(this.gameObject);
    }

    public void Save()
    {
        // Player containers from PTC assigner.
        List<PlayerContainer> playerContainers = ptcAssigner.GetPlayerContainers();

        assignedPlayers = ptcAssigner.AssignedPlayers;
        for (int i = 0; i < assignedPlayers; ++i)
        {
            transferedPlayerContainers.Add(playerContainers[i]);
        }

    }

    public List<PlayerContainer> GetTransferedPlayerContainers()
    {
        return transferedPlayerContainers;
    }

    public int AssignedPlayers
    {
        get { return assignedPlayers; }
    }
}
