/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       PlayerHandler.cs
 * Purpose:     Holds all the components and information relative to the player.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using UnityEngine;
using XboxCtrlrInput;

public class PlayerHandler : MonoBehaviour
{
    [Header("Attributes")]
    public int mechHealth;
    public int coreHealth;

    [Header("References")]
    public GameObject mechObject;
    public GameObject coreObject;
    public RectTransform crosshairTransform;
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public GameObject viewModelObject;
    public GameObject mechModelObject;
    public GameManager gameManager;

    // ----------------------------------------------- //
    private MechController mechController;
    private int mechMaxHealth, coreMaxHealth;
    private XboxController assignedController;
    private Vector3 currentVelocity = Vector3.zero;
    private CharacterController characterController;
    private ImpactReceiver impactReceiver;
    private Rigidbody coreRB;
    private Transform mechTransform, coreTransform;
    private int playerID;
    private bool isAlive;
    private bool isGrounded;
    private bool isInvulnerable = false;
    private StateManager stateManager;
    private PlayerStatistics playerStats;
    private bool isControllable = true;
    private int randomStationIndex = 0;
    // ----------------------------------------------- //

    private void Awake()
    {
        // Setting max values to inspector values.
        mechMaxHealth = mechHealth;
        coreMaxHealth = coreHealth;

        stateManager = GetComponent<StateManager>();
        playerStats = GetComponent<PlayerStatistics>();

        coreRB = coreObject.GetComponentInChildren<Rigidbody>();
        coreTransform = coreObject.GetComponent<Transform>();

        mechController = mechObject.GetComponent<MechController>();
        characterController = mechObject.GetComponent<CharacterController>();
        mechTransform = mechObject.GetComponent<Transform>();
        impactReceiver = mechObject.GetComponent<ImpactReceiver>();
    }

    private void Update()
    {
        // Checking the mechs health ----
        if (mechHealth <= 0)
        {
            stateManager.SetState(StateManager.PLAYER_STATE.Core);
            mechHealth = mechMaxHealth;
        }

        // Checking the balls health:
        if (coreHealth <= 0)
        {
            // Respawning player at random location.
            RespawnAtRandomStation();
        }
    }
    
    // A function to respawn the player at a random respawn staion.
    public void RespawnAtRandomStation()
    {
        // Setting the core to the death state.
        playerStats.HasDied();

        // Getting the random mech station.
        randomStationIndex = Random.Range(0, RespawnArray.instance.mechRespawnStations.Count - 1);

        // Spawning the player at that location.
        RespawnArray.instance.mechRespawnStations[randomStationIndex].SpawnPlayer(this);
    }

    public StateManager.PLAYER_STATE CurrentState
    {
        get { return stateManager.CurrentState; }
        set { stateManager.SetState(value); }
    }

    public CharacterController MechCharacterController
    {
        get { return characterController; }
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

    public XboxController AssignedController
    {
        get { return assignedController; }
        set { assignedController = value; }
    }

    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }

    public bool HasAssignedController
    {
        get { return assignedController > 0; }
    }

    public PlayerStatistics PlayerStats
    {
        get { return playerStats; }
    }

    public bool IsControllable
    {
        get { return isControllable; }
        set { isControllable = value; }
    }

    public MechController MechController
    {
        get { return mechController; }
    }

    public ImpactReceiver MechImpactRecevier
    {
        get { return impactReceiver; }
    }

    public Camera ThirdPersonCamera
    {
        get { return thirdPersonCamera; }
    }

    public StateManager StateManager
    {
        get { return stateManager; }
    }

    public bool IsInvulnerable
    {
        get { return isInvulnerable; }
        set { isInvulnerable = value; }
    }

    public GameManager GameManager
    {
        get { return gameManager; }
        set { gameManager = value; }
    }
}
