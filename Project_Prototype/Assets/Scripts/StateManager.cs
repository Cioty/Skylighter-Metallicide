using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum PLAYER_STATE
    {
        Mech,
        Ball,
        Count
    }

    [Header("Camera")]
    public Camera playerCamera;
    public GameObject cameraGroup;
    public GameObject firstPersonCameraPos, thirdPersonCameraPos;


    [Header("Camera")]
    public GameObject mechObject;
    public GameObject ballObject;
    public GameObject mechEjectEffect;
    public float ejectHeight = 10.0f, ejectForce = 10.0f;

    // Private variables.
    private Vector3 targetPos = new Vector3();
    private Vector3 currentPos = new Vector3();
    private bool moveCamera = false;
    private float cameraSmoothTime = 0;
    private PLAYER_STATE currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PLAYER_STATE.Mech;
        cameraGroup.transform.parent = mechObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.LeftShift))
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
                if(!moveCamera)
                {
                    moveCamera = false;

                    // Swapping camera controls.
                    playerCamera.GetComponent<FP_MouseLook>().enabled = false;
                    playerCamera.GetComponent<TP_MouseLook>().enabled = true;

                    // Setting the camera groups transform to be the ball.
                    cameraGroup.transform.parent = ballObject.transform;

                    // Swapping movement controls.
                    mechObject.GetComponent<MovementController>().enabled = false;
                    mechObject.gameObject.transform.parent = null;

                    ballObject.SetActive(true);
                    ballObject.GetComponent<Ball_Movement>().enabled = true;
                    playerCamera.gameObject.transform.parent = ballObject.transform;

                    // Turning on the external mech in the cameras culling mask.
                    playerCamera.cullingMask |= 1 << LayerMask.NameToLayer("External_Mech");

                    // Disabling the mechs HUD.
                    mechObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);

                    // Updating the current state.
                    currentState = PLAYER_STATE.Ball;

                    // Updating the balls postion.
                    ballObject.transform.position = mechObject.transform.position + (new Vector3(0, ejectHeight, 0));
                }

                // Disable the mech and fp camera object.
                //mechObject.SetActive(false);
                //firstPersonCamera.SetActive(false);

                // Playing particle effect.
                //GameObject explosionObject = Instantiate(mechEjectEffect, mechObject.transform.position, mechObject.transform.rotation);
                //Destroy(explosionObject, 1.9f);

                // Enable the ball and tp camera object.
                //ballObject.SetActive(true);
                //thirdPersonCamera.SetActive(true);

                // Resetting the balls velocity.
                //Rigidbody ballRB = ballObject.GetComponent<Rigidbody>();
                //ballRB.velocity = Vector3.zero;

                // Adding some force to the ball to shoot it up on switch.
                //Vector3 ejectDirection = -transform.forward * 0.5f;
                //ballRB.AddForce(ejectDirection * ejectForce, ForceMode.Impulse);
                break;
            
            case PLAYER_STATE.Mech:
                //// Disable the ball and tp camera object.
                //ballObject.SetActive(false);
                //thirdPersonCamera.SetActive(false);

                //// Enable the mech and fp camera object.
                //mechObject.SetActive(true);
                //firstPersonCamera.SetActive(true);

                //// Updating the current state.
                //currentState = PLAYER_STATE.Mech;

                //// Updating the mechs postion.
                //mechObject.transform.position = ballObject.transform.position;
                break;
        }
    }

    // Moving camera between the two positions.
    private bool LerpCamera(GameObject target, Vector3 posA, Vector3 posB, float time)
    {
        Vector3 smoothedPosition = Vector3.Lerp(posA, posB, time);
        playerCamera.transform.position = smoothedPosition;
        playerCamera.transform.LookAt(target.transform);

        if ((smoothedPosition - posB).magnitude <= 0)
            return true;
        else
            return false;
    }
}
