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

    [Header("References")]
    public GameObject mechObject;
    public GameObject coreObject;
    public RectTransform crosshairTransform;
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public GameObject viewModelObject;
    public GameObject mechModelObject;

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
    // ----------------------------------------------- //

    [Header("Boost Meter")]
    public float boostRegen = 2.0f;

    // Three is the only amount they'll get
    private int boostPoints = 3;

    private float boostTime = 0.0f;

    private float zero = 0.0f;

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
            stateManager.SetState(StateManager.PLAYER_STATE.Core);
            mechHealth = mechMaxHealth;
        }

        // Checking the balls health:
        if (coreHealth <= 0)
        {
            
        }

        if (boostPoints < 3)
        {
            if (boostTime < boostRegen)
            {
                boostTime += 1.0f * Time.deltaTime;
            }

            if (boostTime > boostRegen)
            {
                boostTime = zero;
                boostPoints++;
            }
        }
    }

    public StateManager.PLAYER_STATE CurrentState
    {
        get { return stateManager.CurrentState; }
        set { stateManager.SetState(value); }
    }

    public CharacterController MechCharacterController
    {
        get { return MechCharacterController; }
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

    public int BoostPoints
    {
        get { return boostPoints; }
        set { boostPoints = value; }
    }
}
