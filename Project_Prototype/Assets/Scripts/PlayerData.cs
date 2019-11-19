/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       PlayerData.cs
 * Purpose:     Serves as a packet of data that the next loaded scene can read
 *              from. This holds the information about the players that we'll
 *              need to spawn them in.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/

using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public PTCAssigner ptcAssigner;
    private int assignedPlayers;
    private List<PlayerContainer> transferedPlayerContainers = new List<PlayerContainer>();
    public static PlayerData instance;
    private bool startInDebugMode = false;
    private int debugPlayerCount = 0;
    private SplitScreenMode currentSplitScreenMode;

    public enum SplitScreenMode
    {
        SINGLE,
        DOUBLE,
        TRIPLE,
        QUAD,
    }

    private void Awake()
    {
        instance = this;

        currentSplitScreenMode = SplitScreenMode.SINGLE;
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

    public bool StartInDebugMode
    {
        get { return startInDebugMode; }
        set { startInDebugMode = value; }
    }

    public int DebugPlayerCount
    {
        get { return debugPlayerCount; }
        set { debugPlayerCount = value; }
    }

    public SplitScreenMode CurrentSplitScreenMode
    {
        get { return currentSplitScreenMode; }
        set { currentSplitScreenMode = value; }
    }
}
