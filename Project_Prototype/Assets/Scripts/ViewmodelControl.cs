using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerHandler playerStats;

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
    // Rigidbody rb;

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
    [Header("Sway Stuff")]
    public float maxSwayX = 30.0f;
    public float sway = 4.0f;
    public float smooth = 3.0f;
    public float maxSwayY = 30.0f;

    [Header("UI")]
    public Image healthBar;
    public Renderer dashChargeRenderer;
    private Material dashMat;
    private float lastHealthChecked = 0f;

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
        controller = this.GetComponent<CharacterController>();

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

        dashMat = dashChargeRenderer.materials[1];
        dashMat.shader = Shader.Find("Shader Graphs/Dash_Shader");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
       
        CameraSway();

        if(lastHealthChecked != playerStats.mechHealth)
        {
            if(playerStats.mechHealth > 0)
                healthBar.fillAmount = playerStats.mechHealth / 100f;
            else
                healthBar.fillAmount = 0f;

            lastHealthChecked = playerStats.mechHealth;
        }
        switch (playerStats.BoostPoints)
        {
            case 0:
                dashMat.SetFloat("_Light1", 0.0f);
                dashMat.SetFloat("_Light2", 0.0f);
                dashMat.SetFloat("_Light3", 0.0f);
                break;
            case 1:
                dashMat.SetFloat("_Light1", 1.0f);
                dashMat.SetFloat("_Light2", 0.0f);
                dashMat.SetFloat("_Light3", 0.0f);
                break;

            case 2:
                dashMat.SetFloat("_Light1", 1.0f);
                dashMat.SetFloat("_Light2", 1.0f);
                dashMat.SetFloat("_Light3", 0.0f);
                break;

            case 3:
                dashMat.SetFloat("_Light1", 1.0f);
                dashMat.SetFloat("_Light2", 1.0f);
                dashMat.SetFloat("_Light3", 1.0f);
                break;
        }
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

        if (rotationDifference.y > maxSwayY)
        {
            rotationDifference.y = maxSwayY;
        }
        if (rotationDifference.y < -maxSwayY)
        {
            rotationDifference.y = -maxSwayY;
        }

        // New rotation to lerp towards || Rotation Y is used to set the Vertical axis.
        // Horizontal rotation uses rotation difference to slowly iter back to normal
        Quaternion rotateView = Quaternion.Euler(viewModelTransform.localRotation.y + rotationDifference.y, viewModelTransform.localRotation.x + -rotationDifference.x, 0.0f);
      
        viewModelTransform.localRotation = Quaternion.Slerp(viewModelTransform.localRotation, rotateView, Time.deltaTime * smooth);       

        // Store this frame's rotation for the next frame.
        lastRotation = newRotation;
    }
}
