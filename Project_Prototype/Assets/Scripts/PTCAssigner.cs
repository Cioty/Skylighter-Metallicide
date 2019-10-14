using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class PTCAssigner : MonoBehaviour
{
    public GameObject playerContainersGroup;
    public GameObject allControllersConnectedScreen;
    public static bool controllerFound = false;


    private int connectedControllers = 0;
    private int assignedPlayers = 0;
    private bool isAtConnectScreen = false;
    private bool hasSearchedForControllers = false;
    private List<int> yetToBeConnectedList = new List<int>();
    private List<PlayerContainer> playerContainers = new List<PlayerContainer>();
    private bool allControllersConnected = false;


    // Start is called before the first frame update
    void Start()
    {
        // Search for controllers:
        if (!hasSearchedForControllers)
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
        for (int i = 0; i < connectedControllers; ++i)
            yetToBeConnectedList.Add(i);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAtConnectScreen)
        {
            // check for input
            if (!allControllersConnected)
            {
                for (int c = 0; c < yetToBeConnectedList.Count; ++c)
                {
                    XboxController controller = (XboxController)c;
                    if (XCI.GetButtonUp(XboxButton.A, controller))
                    {
                        this.AddController(c, controller);
                    }
                }

                if (yetToBeConnectedList.Count == 0 && assignedPlayers > 0)
                {
                    allControllersConnectedScreen.SetActive(true);
                    allControllersConnected = true;
                }
            }
            else
            {
                if (XCI.GetButtonUp(XboxButton.A, XboxController.All))
                {
                    SceneManager.LoadScene("Map01", LoadSceneMode.Single);
                }

                if (XCI.GetButtonUp(XboxButton.B, XboxController.All))
                {
                    allControllersConnected = false;
                    allControllersConnectedScreen.SetActive(false);
                }
            }
        }
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

    private void AddController(int id, XboxController controller)
    {
        // If there's a container empty, then add a player into it.
        GameObject containerGO = this.FindNextEmptyContainer().gameObject;
        if (containerGO)
        {
            PlayerContainer container = containerGO.GetComponent<PlayerContainer>();
            containerGO.GetComponent<Renderer>().material.color = Color.green;
            container.ID = id;
            container.Controller = controller;
            container.HasPlayer = true;
            yetToBeConnectedList.Remove(id);
            ++assignedPlayers;
        }
        else
        {
            Debug.Log("Can't find an empty container.");
        }
    }

    public bool IsAtConnectScreen
    {
        get
        {
            return isAtConnectScreen;
        }
        set
        {
            isAtConnectScreen = value;
        }
    }
}
