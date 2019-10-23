using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerHandler : MonoBehaviour
{
    public int mechHealth;
    public int coreHealth;
    public GameObject mechObject;
    public GameObject coreObject;
    private int mechMaxHealth, coreMaxHealth;
    public RectTransform crosshairTransform;
    public Camera firstPersonCamera;
    private XboxController assignedController;
    private Vector3 currentVelocity = Vector3.zero;
    private Rigidbody mechRB, coreRB;
    private Transform mechTransform, coreTransform;
    private int playerID;
    private bool isAlive;
    private bool isGrounded;
    private StateManager stateManager;

    private void Awake()
    {
        // Setting max values to inspector values.
        mechMaxHealth = mechHealth;
        coreMaxHealth = coreHealth;

        stateManager = GetComponent<StateManager>();

        coreRB = coreObject.GetComponentInChildren<Rigidbody>();
        coreTransform = coreObject.GetComponent<Transform>();

        mechRB = mechObject.GetComponent<Rigidbody>();
        mechTransform = mechObject.GetComponent<Transform>();
    }

    private void Update()
    {
        if (mechTransform.position.y < -30)
            this.mechHealth = 0;

        if (coreTransform.position.y < -30)
            this.coreHealth = 0;
    }

    public StateManager.PLAYER_STATE CurrentState
    {
        get { return stateManager.CurrentState; }
        set { stateManager.SetState(value); }
    }

    public Rigidbody GetCurrentRigidBody()
    {
        switch(stateManager.CurrentState)
        {
            case StateManager.PLAYER_STATE.Core:
                return coreRB;

            case StateManager.PLAYER_STATE.Mech:
                return mechRB;
        }

        return null;
    }

    public Rigidbody MechRigidbody
    {
        get { return mechRB; }
    }

    public Rigidbody CoreRigidbody
    {
        get { return coreRB; }
    }

    public Transform MechTransform
    {
        get { return mechTransform; }
    }

    public Transform CoreTransform
    {
        get { return coreTransform; }
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

    public bool IsGrounded
    {
        get { return isGrounded;  }
        set { isGrounded = value; }
    }

}
