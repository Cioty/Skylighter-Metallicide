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
    public GameObject mechEjectEffect;
    public Canvas hudCanvas;

    // Unused atm:
    private GameObject firstPersonCameraPos;
    private GameObject thirdPersonCameraPos;

    [Header("Eject Properties")]
    public KeyCode debugEjectKey;
    public Transform ejectDirection;
    public float ejectHeight = 5.0f, ejectForce = 10.0f;

    // Private references & variables.
    private GameObject mechObject;
    private GameObject coreObject;
    private Vector3 targetPos = new Vector3();
    private Vector3 currentPos = new Vector3();
    private bool moveCamera = false;
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
        if (moveCamera)
        {
            // Lerping camera to 3rd person perspective.
            if (currentState == PLAYER_STATE.Mech)
            {
                targetPos = thirdPersonCameraPos.transform.position;
                currentPos = firstPersonCameraPos.transform.position;

                cameraSmoothTime += Time.deltaTime;
                if (cameraSmoothTime > 1)
                    cameraSmoothTime = 1;

                bool result = LerpCamera(coreObject, currentPos, targetPos, cameraSmoothTime);
                if (result)
                {
                    Debug.Log("Completed camera transition.");
                    moveCamera = false;
                }

                SetState(PLAYER_STATE.Core);

            }
            // Going back to first person.
            else if (currentState == PLAYER_STATE.Core)
            {
                targetPos = firstPersonCameraPos.transform.position;
                currentPos = thirdPersonCameraPos.transform.position;

                // If the current pos is the target pos, then we want to exit out of the function.
                if (currentPos == targetPos)
                {
                    moveCamera = false;
                    return;
                }

                cameraSmoothTime -= Time.deltaTime;
                if (cameraSmoothTime < 0)
                    cameraSmoothTime = 0;

                this.LerpCamera(mechObject, thirdPersonCameraPos.transform.position, firstPersonCameraPos.transform.position, cameraSmoothTime);
            }
        }
    }

    public void SetState(PLAYER_STATE state)
    {
        currentState = state;
        switch (currentState)
        {
            case PLAYER_STATE.Core:
                //Playing particle effect.
                GameObject explosionObject = Instantiate(mechEjectEffect, mechObject.transform.position, mechObject.transform.rotation);
                Destroy(explosionObject, 1.9f);


                // Swapping active objects.
                // Updating the core postion.
                coreObject.SetActive(true);
                playerHandler.coreModelObject.transform.position = ejectDirection.position;
                mechObject.SetActive(false);
                playerHandler.CoreRigidbody.AddForce(ejectDirection.forward * ejectForce, ForceMode.Impulse);
                hudCanvas.worldCamera = playerHandler.ThirdPersonCamera;

                break;

            case PLAYER_STATE.Mech:
                // Swapping active objects.
                coreObject.SetActive(false);

                // Updating the mechs postion.
                mechObject.SetActive(true);

                hudCanvas.worldCamera = playerHandler.FirstPersonCamera;
                break;
        }
    }

    // Moving camera between the two positions.
    private bool LerpCamera(GameObject target, Vector3 posA, Vector3 posB, float time)
    {
        Vector3 smoothedPosition = Vector3.Lerp(posA, posB, time);
        if ((smoothedPosition - posB).magnitude <= 0)
            return true;
        else
            return false;
    }

    public PLAYER_STATE CurrentState
    {
        get { return currentState;  }
        set { currentState = value; }
    }
}
