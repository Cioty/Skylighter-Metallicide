/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       PTCAssigner.cs (Player To Controller Assigner)
 * Purpose:     Uses XInput to search for connected controllers, and handles
 *              the input of the controller selection screen.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 *             - Currently assigns the controllers in any screen other. 
 *             - (eg, controller 4 can player in the topleft screen in 4 player)
 * 
 *===========================================================================*/
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class PTCAssigner : MonoBehaviour
{
    [Header("References")]
    public GameObject playerContainersGroup;
    public GameObject allControllersConnectedScreen;

    [Header("Debug Mode")]
    [Tooltip("Keep this off to start game normally!")]
    public bool startInDebugMode = false;
    public KeyCode debugStartKey;
    public int debugPlayerCount = 0;

    // Private variables:
    private int connectedControllers = 0;
    private int assignedPlayers = 0;
    private bool isAtConnectScreen = false;
    private bool hasSearchedForControllers = false;
    private List<XboxController> connectedControllerList = new List<XboxController>();
    private List<PlayerContainer> playerContainers = new List<PlayerContainer>();
    public static bool controllerFound = false;

    // Start is called before the first frame update
    void Start()
    {
        // Search for controllers:
        if (!hasSearchedForControllers && !startInDebugMode)
        {
            hasSearchedForControllers = true;

            connectedControllers = XCI.GetNumPluggedCtrlrs();
            if (connectedControllers == 1)
            {
                Debug.Log("Only " + connectedControllers + " Xbox controller plugged in.");
                controllerFound = true;
            }
            else if (connectedControllers == 0)
                Debug.Log("No Xbox controllers plugged in!");
            else
            {
                Debug.Log(connectedControllers + " Xbox controllers plugged in.");
                controllerFound = true;
            }

            XCI.DEBUG_LogControllerNames();
        }
        
        // Adding the player containers to a list.
        for (int i = 0; i < playerContainersGroup.transform.childCount; ++i)
            playerContainers.Add(playerContainersGroup.transform.GetChild(i).transform.gameObject.GetComponent<PlayerContainer>());

        // Adding to the yet to bed added list
        for (int c = 1; c < connectedControllers + 1; ++c)
        {
            XboxController xboxController = ((XboxController)c);

            if (xboxController == XboxController.All)
                continue;

            connectedControllerList.Add(xboxController);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Quick start the game in debug mode.
        if (Input.GetKeyDown(debugStartKey) && startInDebugMode)
        {
            PlayerData.instance.DebugPlayerCount = debugPlayerCount;
            PlayerData.instance.StartInDebugMode = true;
            StartGame();
        }

        if (isAtConnectScreen)
        {
            if (connectedControllerList.Count > 0)
            {
                for (int c = 0; c < connectedControllerList.Count; ++c)
                {
                    XboxController xboxController = connectedControllerList[c];

                    // If xbox controller is all then skip:
                    if (xboxController == XboxController.All)
                        continue;

                    // Listening to avalible controllers that have yet to be connected:
                    if (XCI.GetButtonUp(XboxButton.B, xboxController))
                    {
                        this.RemoveController(xboxController);
                    }

                    //// If the container already has a player, then skip:
                    //if (playerContainers[c].HasPlayer)
                    //    continue;

                    // Listening to avalible controllers that have yet to be connected:
                    if (XCI.GetButtonUp(XboxButton.A, xboxController))
                    {
                        this.AddController(xboxController);
                    }
                }
            }

            if (assignedPlayers > 1)
            {
                PlayerData.instance.CurrentSplitScreenMode = ((PlayerData.SplitScreenMode)assignedPlayers - 1);
                allControllersConnectedScreen.SetActive(true);

                if (XCI.GetButtonUp(XboxButton.Start, XboxController.All))
                {
                    StartGame();
                }
            }
        }
    }

    private void StartGame()
    {
        PlayerData.instance.Save();
        SceneManager.LoadScene("Map02", LoadSceneMode.Single);
    }

    private PlayerContainer GetContainerByID(int ID)
    {
        return playerContainers[ID];
    }

    // Loops through the container list and returns the first container without a player.
    private PlayerContainer FindNextEmptyContainer()
    {
        for(int i = 0; i < playerContainers.Count; ++i)
        {
            // Checking if the container has a player, and skipping the return if it does:
            PlayerContainer container = playerContainers[i].GetComponent<PlayerContainer>();
            if (container.HasPlayer)
                continue;

            // Returning the empty container.
            return container;
        }

        // Returns null if no empty container is found.
        return null;
    }

    private void AddController(XboxController controller)
    {
        int id = ((int)controller) - 1;
        // If there's a container empty, then add a player into it. (Not sure if we should do this, or just match the ID to the connected controller).
        GameObject containerGO = this.GetContainerByID(id).gameObject;
        if (containerGO)
        {
            PlayerContainer container = containerGO.GetComponent<PlayerContainer>();
            container.ID = id;
            container.Controller = controller;
            container.HasPlayer = true;
            container.ToggleMech();
            ++assignedPlayers;
        }
        else
        {
            Debug.Log("Can't find an empty container.");
        }
    }

    private void RemoveController(XboxController controller)
    {
        int id = ((int)controller) - 1;
        GameObject containerGO = this.GetContainerByID(id).gameObject;
        if (containerGO)
        {
            PlayerContainer container = containerGO.GetComponent<PlayerContainer>();
            container.ID = -1;
            container.Controller = ((XboxController)0);
            container.HasPlayer = false;
            container.ToggleMech();
            --assignedPlayers;
        }
        else
        {
            Debug.Log("Can't find an empty container.");
        }
    }

    public List<PlayerContainer> GetPlayerContainers()
    {
        return playerContainers;
    }

    public int AssignedPlayers
    {
        get { return assignedPlayers; }
    }

    public int ConnectedControllers
    {
        get { return connectedControllers; }
    }

    public bool IsAtConnectScreen
    {
        get { return isAtConnectScreen;  }
        set { isAtConnectScreen = value; }
    }
}
