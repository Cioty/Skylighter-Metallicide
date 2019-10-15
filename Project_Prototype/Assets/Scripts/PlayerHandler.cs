using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerHandler : MonoBehaviour
{
    public int mechHealth;
    public int coreHealth;
    private int mechMaxHealth, coreMaxHealth;
    public RectTransform crosshairTransform;
    public Camera firstPersonCamera;
    private XboxController assignedController;
    private Vector3 currentVelocity = Vector3.zero;
    private int playerID;
    private bool isAlive;

    private void Awake()
    {
        // Setting max values to inspector values.
        mechMaxHealth = mechHealth;
        coreMaxHealth = coreHealth;
    }

    public RectTransform CrosshairTransform
    {
        get { return crosshairTransform; }
        set { crosshairTransform = value; }
    }

    public int MechHealth
    {
        get { return mechHealth; }
        set { mechHealth = value; }
    }

    public int CoreHealth
    {
        get { return coreHealth; }
        set { coreHealth = value; }
    }


    public int MaxMechHealth
    {
        get { return mechHealth; }
    }

    public int MaxCoreHealth
    {
        get { return coreHealth; }
    }


    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

    public int ID
    {
        get { return playerID; }
        set { playerID = value; }
    }

    public Camera FirstPersonCamera
    {
        get { return firstPersonCamera; }
    }

    public Vector3 CurrentVelocity
    {
        get { return currentVelocity; }
        set { currentVelocity = value; }
    }

    public GameObject GameObject
    {
        get { return this.gameObject; }
    }

    public XboxController AssignedController
    {
        get { return assignedController; }
        set { assignedController = value; }
    }

}
