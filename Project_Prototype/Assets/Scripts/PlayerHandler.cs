﻿/*=============================================================================
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

    [Header("Object References")]
    public GameObject mechObject;
    public GameObject coreObject;
    public RectTransform crosshairTransform;
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public GameObject viewModelObject;
    public GameObject mechModelObject;
    public GameManager gameManager;
    public ScreenShake firstPersonScreenShake;
    public RocketJump rocketJump;
    public string playerViewMask;

    [Header("Screen Shake")]
    public float shakeDuration = 0.5f;
    public float shakeForce = 0.1f;

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

    [Header("Boost Meter")]
    // Amount of times you can boost
    private int boostPoints = 3;

    // The things that will be used to regenerate boost uses
    public float boostRegen = 2.0f;
    private float boostTime;


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
        // Checking the mechs health:
        if (mechHealth <= 0)
        {
            // Changing the state of the player to the core:
            stateManager.SetState(StateManager.PLAYER_STATE.Core);

            // Resetting the mech's health:
            mechHealth = mechMaxHealth;
        }

        // Checking the balls health:
        if (coreHealth <= 0)
        {
            // Respawning player at random location.
            RandomSpawn_FromDeath();
        }

        // Regenerate Boost Meter:
        if (boostPoints < 3)
        {
            boostTime += 1 * Time.deltaTime;
        }
        if (boostTime > boostRegen)
        {
            boostTime = 0.0f;
            boostPoints++;
        }
    }

    public void RandomSpawn_Unactive()
    {
        // Getting the random mech station.
        randomStationIndex = Random.Range(0, RespawnArray.instance.mechRespawnStations.Count);

        // Spawning the player at that location.
        RespawnArray.instance.mechRespawnStations[randomStationIndex].SpawnPlayer(this);
    }

    // A function to respawn the player at a random respawn staion.
    public void RandomSpawn_FromDeath()
    {
        // Setting the core to the death state.
        playerStats.HasDied();

        // Resetting player's health.
        this.mechHealth = MaxMechHealth;
        this.coreHealth = MaxCoreHealth;

        // Getting the random mech station.
        randomStationIndex = Random.Range(0, RespawnArray.instance.mechRespawnStations.Count);

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

    public int BoostPoints
    {
        get { return boostPoints; }
        set { boostPoints = value; }
    }

    // Returns mech health:
    public int Mech_TakeDamage(int damage)
    {
        StartCoroutine(firstPersonScreenShake.Shake(shakeDuration, shakeForce));
        this.mechHealth -= damage;
        return this.mechHealth;
    }

    public int Core_TakeDamage(int damage)
    {
        this.coreHealth -= damage;
        return this.coreHealth;
    }

    public bool IsInMech()
    {
        return CurrentState == StateManager.PLAYER_STATE.Mech;
    }

    public bool IsInCore()
    {
        return CurrentState == StateManager.PLAYER_STATE.Core;
    }

    public ScreenShake Mech_ScreenShake
    {
        get { return firstPersonScreenShake; }
    }

    public RocketJump Mech_RocketJump
    {
        get { return rocketJump; }
    }
}
