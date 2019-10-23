using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewmodelControl : MonoBehaviour
{   
    // Something to keep track of the currentCurrentposition
    Vector3 currentCamPos;
    Vector3 newCamPosition;
    
    // Variables needed
    public GameObject viewModel;
    public GameObject player;
    public GameObject Camera;

    // Camera info and speed
    FirstPersonCamera MechCamera;
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

    private void Awake()
    {
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
        viewModelRotation = player.transform.rotation;

        origX = viewModelRotation.x;
        origY = viewModelRotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        ApplyBob();
        CameraSway();   
    }

    // When the Mech moves
    void ApplyBob()
    {
        //if (controller.velocity.magnitude > 0.0f)
        //{
        //    camOsillation = Mathf.Sin(timer * (2 * Mathf.PI));          
        //    calcPosition.y = camOsillation * 0.5f;            
        //}
        //else
        //{            
        //    calcPosition.y = InitY;
        //}
        //camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, calcPosition, Time.deltaTime);
    }

    // Camera sway
    void CameraSway()
    {
        camDirNormal = (cameraDirection - camTransform.position).normalized;
        viewModelDirNormal = (viewModelDir - viewModelTransform.position).normalized;


        // Check to see if they're different
        if (camDirNormal != viewModelDirNormal)
        {
            viewModelRotation = Quaternion.Lerp(viewModelRotation, camTransform.localRotation, Time.deltaTime);
        }

        // The camera view is not the same as camera's facing direction
        //if (viewModelRotation != camTransform.localRotation)
        //{
        //    viewModelRotation = Quaternion.Lerp(viewModelRotation, camTransform.localRotation, Time.deltaTime);
        //}
    }
}
