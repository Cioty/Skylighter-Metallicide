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
    public enum PLAYER_STATE
    {
        Mech,
        Core,
        Count
    }

    [Header("References")]
    public GameObject firstPersonCameraPos;
    public GameObject thirdPersonCameraPos;
    public GameObject mechObject;
    public GameObject coreManager;
    public GameObject coreGraphic;
    public GameObject mechEjectEffect;
    public PlayerHandler playerHandler;

    [Header("Eject Properties")]
    public KeyCode debugEjectKey;
    public float ejectHeight = 5.0f, ejectForce = 10.0f;

    // Private variables.
    private Vector3 targetPos = new Vector3();
    private Vector3 currentPos = new Vector3();
    private bool moveCamera = false;
    private float cameraSmoothTime = 0;
    private PLAYER_STATE currentState;

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

                bool result = LerpCamera(coreManager, currentPos, targetPos, cameraSmoothTime);
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
        switch (state)
        {
            case PLAYER_STATE.Core:
                //Playing particle effect.
                GameObject explosionObject = Instantiate(mechEjectEffect, mechObject.transform.position, mechObject.transform.rotation);
                Destroy(explosionObject, 1.9f);

                // Swapping active objects.

                // Updating the core postion.
                //playerHandler.CoreRigidbody.velocity = playerHandler.MechCharacterController.velocity;
                coreManager.SetActive(true);
                coreManager.transform.position = mechObject.transform.position;
                mechObject.SetActive(false);
                playerHandler.CoreRigidbody.AddForce(Vector3.up * ejectForce, ForceMode.Impulse);
                break;

            case PLAYER_STATE.Mech:
                // Swapping active objects.
                coreManager.SetActive(false);

                // Updating the mechs postion.
                mechObject.SetActive(true);

                //playerHandler.MechCharacterController.enabled = false;
                mechObject.transform.position = coreGraphic.transform.position;
                //playerHandler.MechCharacterController.enabled = true;

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
