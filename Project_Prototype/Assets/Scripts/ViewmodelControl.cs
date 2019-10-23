using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewmodelControl : MonoBehaviour
{   
    // Something to keep track of the current position
    Vector3 currentCamPos;
    Vector3 newCamPosition;
    
    // Variables needed
    public GameObject viewModel;
    public GameObject player;
    public GameObject Camera;

    // Camera info and speed
    //FirstPersonCamera MechCamera;
    PlayerHandler playerStats;

    // The camera's position and rotation
    Transform camTransform;
    private float InitX, InitY;
    Vector3 calcPosition;
    Quaternion calcRotation;

    // Camera Direction for the swaying
    Vector3 cameraDirection;
    Vector3 camDirNormal;

    // Player's velocity
    CharacterController controller;

    // Bobbing curve
    float camOsillation;

    // Timer that goes through the sine wave
    float timer;

    // Viewmodel Transform
    private Transform viewModelTransform;
    
    Quaternion viewModelRotation;
    private float origX, origY;

    // View model direction for swaying
    Vector3 viewModelDir;
    Vector3 viewModelDirNormal;

    // View model sway variables
    float maxSwayX = 30.0f;
    float sway = 4.0f;
    float smooth = 3.0f;
    float maxSwayY = 80f;


    // Get the first person Camera script
    FirstPersonCamera mouseRotation;
    Vector3 lastRotation = Vector3.zero;
    Vector3 newRotation = Vector3.zero;
    Vector3 rotationDifference;

    // Camera speed zoom
    Vector3 currentVelocity = Vector3.zero;
    Vector3 lastVelocity = Vector3.zero;
    Vector3 directionThrowBack = Vector3.zero;

    private void Awake()
    {
        mouseRotation = GetComponent<FirstPersonCamera>();

        controller = GetComponentInParent<CharacterController>();
        playerStats = player.GetComponent<PlayerHandler>();

        // Camera's transform
        camTransform = Camera.transform;
        cameraDirection = Camera.transform.position + (Vector3.forward * 10);

        // viewModel Transform
        viewModelTransform = viewModel.transform;
        viewModelDir = viewModel.transform.position + (Vector3.forward * 10);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Camera's initial transform
        InitX = camTransform.localRotation.x;
        InitY = camTransform.localPosition.y;

        calcPosition = camTransform.transform.localPosition;
        calcRotation = camTransform.transform.localRotation;

        // View Model's Initial direction
        viewModelRotation = viewModelTransform.localRotation;

        origX = viewModelRotation.x;
        origY = viewModelRotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        ApplyBob();
        CameraSway();
        CameraDrag();
    }

    // When the Mech moves
    void ApplyBob()
    {
        if (controller.velocity.magnitude > 0.0f)
        {
            // It's just a Sine wave the goes back and forth from -1 to +1
            camOsillation = Mathf.Sin(timer * (2 * Mathf.PI));
            
            // Make the position whatever the sine wave is up to
            calcPosition.y = camOsillation * 0.5f;            
        }
        else
        {            
            calcPosition.y = InitY;
        }
        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, calcPosition, Time.deltaTime);
    }

    // Camera sway
    // View Model's rotation tries to match camera rotation
    void CameraSway()
    {
        // mouseLook from the camera
        newRotation = mouseRotation.MouseLook;
        // The difference between the this frame and last frame's mouse look distance
        // Scaled by a sway amount
        rotationDifference = (newRotation - lastRotation) * sway;

        // Sway limitor
        if (rotationDifference.x > maxSwayX)
        {                      
            rotationDifference.x = maxSwayX;
        }                      
        if (rotationDifference.x < -maxSwayX)
        {                      
            rotationDifference.x = -maxSwayX;
        }

        //if (rotationDifference.y > maxSwayY)
        //{
        //    rotationDifference.y = maxSwayY;
        //}
        //if (rotationDifference.y < -maxSwayY)
        //{
        //    rotationDifference.y = -maxSwayY;
        //}

        // New rotation to lerp towards || Rotation Y is used to set the Vertical axis.
        // Horizontal rotation uses rotation differents to slowly iter back to normal
        Quaternion rotateView = Quaternion.Euler(0.0f, viewModelTransform.localRotation.x + -rotationDifference.x, 0.0f) * (Quaternion.AngleAxis(-newRotation.y + -rotationDifference.y, Vector3.right));
      
        viewModelTransform.localRotation = Quaternion.Slerp(viewModelTransform.localRotation, rotateView, Time.deltaTime * smooth);       

        // Store this frame's rotation for the next frame.
        lastRotation = newRotation;
    }

    /*
     * Keep track of what mouseLook.y was last frame
     * Every frame: get the mouseLook.y from camera
     * //Work out difference : turning velocity
     */


    // Ever been in a car that accelerated too fast?
    // Your head gets blown back
    void CameraDrag()
    {
        // The current velocity
        currentVelocity = playerStats.CurrentVelocity;

        // The direction the camera is thrown back (always opposite of where you're heading)
        directionThrowBack = -(currentVelocity - lastVelocity).normalized;

        // Scale the normalised vector by a set distance.
        directionThrowBack = directionThrowBack * 2;

        Vector3 tempPosition = camTransform.position - directionThrowBack;

        // Lerp towards the new position at the rate of the speed.
        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, tempPosition, Time.deltaTime * (currentVelocity - lastVelocity).magnitude);


        // The last frame's velocity
        lastVelocity = currentVelocity;
    }
}
