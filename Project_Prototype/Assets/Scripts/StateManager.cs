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
        Ball,
        Count
    }

    [Header("References")]
    public GameObject firstPersonCameraPos;
    public GameObject thirdPersonCameraPos;
    public GameObject mechObject;
    public GameObject ballObject;
    public GameObject mechEjectEffect;

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
        if(Input.GetKeyUp(debugEjectKey))
        {
            if(currentState == PLAYER_STATE.Ball)
                SetState(PLAYER_STATE.Mech);
            else
                SetState(PLAYER_STATE.Ball);
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

                bool result = LerpCamera(ballObject, currentPos, targetPos, cameraSmoothTime);
                if(result)
                {
                    Debug.Log("Completed camera transition.");
                    moveCamera = false;
                }

                SetState(PLAYER_STATE.Ball);

            }
            // Going back to first person.
            else if (currentState == PLAYER_STATE.Ball)
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
        switch(state)
        {
            case PLAYER_STATE.Ball:
                //Playing particle effect.
                GameObject explosionObject = Instantiate(mechEjectEffect, mechObject.transform.position, mechObject.transform.rotation);
                Destroy(explosionObject, 1.9f);

                // Swapping active objects.
                this.gameObject.GetComponent<CharacterController>().enabled = false;
                mechObject.SetActive(false);
                ballObject.SetActive(true);

                // Updating the current state.
                currentState = PLAYER_STATE.Ball;

                // Updating the balls postion.
                //ballObject.transform.position = mechObject.transform.position + (new Vector3(0, ejectHeight, 0));
                ballObject.GetComponentInChildren<Rigidbody>().AddForce(Vector3.up * ejectForce);
                break;
            
            case PLAYER_STATE.Mech:
                // Swapping active objects.
                this.gameObject.GetComponent<CharacterController>().enabled = true;
                mechObject.SetActive(true);
                ballObject.SetActive(false);

                // Updating the current state.
                currentState = PLAYER_STATE.Mech;

                // Updating the mechs postion.
                this.gameObject.transform.position = ballObject.transform.position;
                break;
        }
    }

    // Moving camera between the two positions.
    private bool LerpCamera(GameObject target, Vector3 posA, Vector3 posB, float time)
    {
        Vector3 smoothedPosition = Vector3.Lerp(posA, posB, time);
        //playerCamera.transform.position = smoothedPosition;
        //playerCamera.transform.LookAt(target.transform);

        if ((smoothedPosition - posB).magnitude <= 0)
            return true;
        else
            return false;
    }
}
