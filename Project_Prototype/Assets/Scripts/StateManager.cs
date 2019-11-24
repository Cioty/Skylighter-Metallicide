/*=============================================================================
 * Game:        Metallicide
 * Version:     Alpha
 * 
 * Class:       StateManager.cs
 * Purpose:     Manages the state of the player.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/

using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Player state enum:
    public enum PLAYER_STATE
    {
        Mech,
        Core,
        Count
    }

    [Header("References")]
    public PlayerHandler playerHandler;
    public ParticleSystem mechEjectEffect;
    public Canvas hudCanvas;
    public Animator mechAnimator;
    public ToggleMechVisibility mechVisibility;
    public Transform firstPersonCameraPos;
    public Transform thirdPersonCameraPos;
    public GameObject tempParent;
    public GameObject originalParent;

    [Header("Eject Properties")]
    public KeyCode debugEjectKey;
    public Transform ejectDirection;
    public float ejectHeight = 5.0f, ejectForce = 10.0f;

    [Header("Eject Camera Transition")]
    public bool thirdPersonSwitch = false;
    public float cameraMoveTime = 1.0f;
    public float timeToEject = 7.0f;

    // Private references & variables.
    private GameObject mechObject;
    private GameObject coreObject;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 currentPos = Vector3.zero;
    private Quaternion targetRot = Quaternion.identity;
    private Quaternion currentRot = Quaternion.identity;
    private bool shouldCameraMove = false;
    private bool startEjectTimer = false;
    private float ejectTimer = 0.0f;
    private float cameraSmoothTime = 0;
    private PLAYER_STATE currentState;

    private void Awake()
    {
        mechObject = playerHandler.mechObject;
        coreObject = playerHandler.coreObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setting the default state of the player.
        currentState = PLAYER_STATE.Mech;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(debugEjectKey) && playerHandler.IsControllable)
        {
            if (currentState == PLAYER_STATE.Core)
                SetState(PLAYER_STATE.Mech);
            else
                SetState(PLAYER_STATE.Core);
        }
    }

    private void FixedUpdate()
    {
        if (shouldCameraMove)
        {
            currentPos = playerHandler.FirstPersonCamera.transform.position;
            targetPos = thirdPersonCameraPos.position;

            currentRot = playerHandler.FirstPersonCamera.transform.rotation;
            targetRot = thirdPersonCameraPos.rotation;

            cameraSmoothTime += Time.deltaTime;
            if (cameraSmoothTime > 1)
                cameraSmoothTime = 1;

            playerHandler.FirstPersonCamera.transform.position = Vector3.Lerp(currentPos, targetPos, cameraSmoothTime / cameraMoveTime);
            playerHandler.FirstPersonCamera.transform.rotation = Quaternion.Slerp(currentRot, targetRot, cameraSmoothTime / cameraMoveTime);
            playerHandler.FirstPersonCamera.transform.LookAt(playerHandler.mechObject.transform);
            if ((playerHandler.FirstPersonCamera.transform.position - targetPos).magnitude <= 0)
            {
                cameraSmoothTime = 0f;
                shouldCameraMove = false;
                startEjectTimer = true;
                Debug.Log("Completed camera transition.");
            }
        }

        if(startEjectTimer)
        {
            ejectTimer += Time.deltaTime;
            if(ejectTimer > timeToEject)
            {
                EjectCore();
                ejectTimer = 0.0f;
                startEjectTimer = false;
            }
        }
    }

    public void SetState(PLAYER_STATE state)
    {
        currentState = state;
        switch (currentState)
        {
            case PLAYER_STATE.Core:
                if (thirdPersonSwitch)
                {
                    if (!mechVisibility.IsShowing)
                    {
                        mechVisibility.Toggle();
                    }
                    playerHandler.FirstPersonCamera.transform.parent = tempParent.transform;
                    playerHandler.coreModelObject.transform.position = ejectDirection.position;
                    shouldCameraMove = true;
                }
                else
                    startEjectTimer = true;

                playerHandler.IsControllable = false;
                mechEjectEffect.Play();
                mechAnimator.SetTrigger("Death");
                break;

            case PLAYER_STATE.Mech:
                if(thirdPersonSwitch)
                {
                    if (mechVisibility.IsShowing)
                    {
                        mechVisibility.Toggle();
                    }
                    playerHandler.FirstPersonCamera.transform.parent = originalParent.transform;
                    playerHandler.FirstPersonCamera.transform.position = firstPersonCameraPos.position;
                    playerHandler.FirstPersonCamera.transform.rotation = firstPersonCameraPos.rotation;
                }

                // Swapping active objects.
                coreObject.SetActive(false);

                // Updating the mechs postion.
                mechObject.SetActive(true);

                hudCanvas.worldCamera = playerHandler.FirstPersonCamera;
                break;
        }
    }

    private void EjectCore()
    {
        playerHandler.coreModelObject.transform.position = ejectDirection.position;
        coreObject.SetActive(true);
        mechObject.SetActive(false);
        playerHandler.CoreRigidbody.AddForce(ejectDirection.forward * ejectForce, ForceMode.Impulse);
        hudCanvas.worldCamera = playerHandler.ThirdPersonCamera;
        playerHandler.IsControllable = true;
    }

    public PLAYER_STATE CurrentState
    {
        get { return currentState;  }
        set { currentState = value; }
    }
}
