using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class TP_MouseLook : MonoBehaviour
{
    public GameObject player;
    public PlayerHandler playerHandler;

    public float rotationSpeed = 30.0f;
    // For Clamping 
    public float minY = -35.0f, maxY = 60.0f;

    // Distance away from Core
    private Vector3 cameraPosDef;


    // Smoothing
    public float smooth;

    private Vector3 offset;
    private Vector3 direction;
    private float defaultDistance;
    public float nearClip = 1.0f;
    public float farClip = 4.0f;
    private float distance;

    // Camera Clipping Raycast
    RaycastHit hit = new RaycastHit();

    Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
    private float mouse_x;
    private float mouse_y;

    private void Awake()
    {
        // Default Camera position is always opposite the core forward transform
        cameraPosDef = transform.position - player.transform.position;
        Cursor.lockState = CursorLockMode.Locked;
        offset = cameraPosDef;
        distance = cameraPosDef.magnitude;
    }

    private void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotation.x = rot.x;
        rotation.y = rot.y;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        CameraClipping();
        Mouse_aiming();
        
    }

    void LateUpdate()
    {
        // transform.position = player.transform.position + cameraPosDef;
        transform.LookAt(player.transform.position);   
    }

    // Purpose: Handles the X & Y axis rotation for camera orbit around the player
    void Mouse_aiming()
    {
        // controller only atm:
        if (playerHandler.HasAssignedController)
        {
            mouse_x = XCI.GetAxis(XboxAxis.RightStickX, playerHandler.AssignedController) * rotationSpeed;
            mouse_y = XCI.GetAxis(XboxAxis.RightStickY, playerHandler.AssignedController) * rotationSpeed;
        }
        else
        {
            mouse_x = Input.GetAxis("Mouse X") * rotationSpeed;
            mouse_y = Input.GetAxis("Mouse Y") * rotationSpeed;
        }

        mouse_y = Mathf.Clamp(mouse_y, minY, maxY); // This keeps the Y-rotation between that angles to prevent full 360 degrees orbit in the Y-Axis

        rotation.x += mouse_y * Time.deltaTime;
        rotation.y += mouse_x * Time.deltaTime;
        
        Quaternion localRotation = Quaternion.Euler(-rotation.x, rotation.y, 0.0f);

        transform.position = player.transform.position + (localRotation * offset);
    }

    /// <summary>
    /// Set's the camera's position to where the line cast is cut off
    /// </summary>
    void CameraClipping()
    {
        direction = (transform.position - player.transform.position).normalized;
        Vector3 oldOffset = Vector3.zero;
        if (Physics.Linecast(player.transform.position, transform.position, out hit))
        {
            // Where the Raycast hit
            oldOffset = offset; // Store previous offset
            distance = Mathf.Clamp(hit.distance * 0.9f, nearClip, farClip);
            offset = offset * (distance / offset.magnitude);
        }
        else
        {
            offset = cameraPosDef;
        }
        //transform.position = Vector3.Lerp(transform.position, transform.position - (offset), Time.deltaTime * 10.0f);
        // offset = Vector3.Lerp(transform.position, transform.position - offset, Time.deltaTime * 10.0f);
    }

  
}
