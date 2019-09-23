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

    public GameObject mechObject, ballObject;
    public GameObject firstPersonCamera, thirdPersonCamera;
    public GameObject mechEjectEffect;
    public float ejectHeight = 10.0f, ejectForce = 10.0f;
    private PLAYER_STATE currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PLAYER_STATE.Mech;
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

    public void SetState(PLAYER_STATE state)
    {
        switch(state)
        {
            case PLAYER_STATE.Ball:

                // Disable the mech and fp camera object.
                mechObject.SetActive(false);
                firstPersonCamera.SetActive(false);

                // Playing particle effect.
                GameObject explosionObject = Instantiate(mechEjectEffect, transform.position, transform.rotation);
                Destroy(explosionObject, 1.9f);

                // Enable the ball and tp camera object.
                ballObject.SetActive(true);
                thirdPersonCamera.SetActive(true);

                // Updating the current state.
                currentState = PLAYER_STATE.Ball;

                // Updating the balls postion.
                ballObject.transform.position = mechObject.transform.position + (new Vector3(0, ejectHeight, 0));

                // Resetting the balls velocity.
                Rigidbody ballRB = ballObject.GetComponent<Rigidbody>();
                ballRB.velocity = new Vector3(0, 0, 0);

                // Adding some force to the ball to shoot it up on switch.
                ballRB.AddForce(Vector3.up * ejectForce, ForceMode.Impulse);
                break;
            
            case PLAYER_STATE.Mech:

                // Disable the ball and tp camera object.
                ballObject.SetActive(false);
                thirdPersonCamera.SetActive(false);

                // Enable the mech and fp camera object.
                mechObject.SetActive(true);
                firstPersonCamera.SetActive(true);

                // Updating the current state.
                currentState = PLAYER_STATE.Mech;

                // Updating the mechs postion.
                mechObject.transform.position = ballObject.transform.position;
                break;
        }
    }
}
