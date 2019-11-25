using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    public float clipSmooth = 0.05f;
    public float returnTime = 0.4f;

    private Vector3 offset; // Distance from Player
    private Vector3 direction; // Direction the Raycast points
    public float defaultDistance = 4.0f;  // Original distance the Player goes back to
    public float nearClip = 1.0f; // How close the Camera can get
    public float farClip = 4.0f; // How far the Camera can get
    private float distance; // The distance the camera 
    private float moveVelocity; // The direction the camera moves in

    // Don't Clip itself
    private GameObject self;   
    public bool protecting { get; private set; } // Used to determine if something is between player and Camera

    // Camera Clipping Raycast
    Ray ray = new Ray(); // Ray for any shapeCast check
    RaycastHit hit = new RaycastHit(); // Single Camera clipping ray check
    private RaycastHit[] hits; // This will be used for the sphereCast
    private RayHitComparer rayHitComparer; // This will compare rayCast collisions

    [Header("Raycast radius")]
    public float sphereCastRadius = 0.1f;

    Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
    private float mouse_x;
    private float mouse_y;

    private void Awake()
    {
        // Default Camera position is always opposite the core forward transform
        cameraPosDef = player.transform.position - (player.transform.forward * defaultDistance);
        Cursor.lockState = CursorLockMode.Locked;
        offset = cameraPosDef;
        distance = cameraPosDef.magnitude;

        // Get this game object for camera clipping security
        self = player;
    }

    private void Start()
    {
        // Camera rotation
        Vector3 rot = transform.localRotation.eulerAngles;
        rotation.x = rot.x;
        rotation.y = rot.y;

        // Ray Hits comparer
        rayHitComparer = new RayHitComparer();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        Mouse_aiming();

    }

    void LateUpdate()
    {
        // transform.position = player.transform.position + cameraPosDef;
        transform.LookAt(player.transform.position);
        CameraClipping();
        //wallClip();
    }

    // Purpose: Handles the X & Y axis rotation for camera orbit around the player
    void Mouse_aiming()
    {
        // controller only atm:
        if(playerHandler.IsControllable)
        {
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
        // Initially set the target distance
        float targetDist = defaultDistance;

        direction = (transform.position - player.transform.position).normalized;
        Vector3 oldOffset = Vector3.zero;

        // Ray cast
        ray.origin = transform.position + transform.forward*sphereCastRadius;
        ray.direction = -transform.forward;

        // Create a bunch of colliders
        Collider[] cols = Physics.OverlapSphere(ray.origin, sphereCastRadius);

        bool initialIntersect = false;
        bool hitSomething = false;

        // loop through all the collisions to check if something we care about
        for (int i = 0; i < cols.Length; i++)
        {
            if ((!cols[i].isTrigger) &&
                !(cols[i].attachedRigidbody != null && cols[i].attachedRigidbody.gameObject == self))
            {
                initialIntersect = true;
                break;
            }
        }

        if (initialIntersect)
        {
            ray.origin += transform.forward * sphereCastRadius;

            // do a raycast and gather all the intersections
            hits = Physics.RaycastAll(ray, defaultDistance - sphereCastRadius);
        }
        else
        {
            // If none, check to see other collisions in all directions
            hits = Physics.SphereCastAll(ray, sphereCastRadius, defaultDistance + sphereCastRadius);
        }

        // Sort the hits by the distance
        Array.Sort(hits, rayHitComparer);

        // Since we don't want to have the nearest distance larger than the nearest
        // Since nothing is larger than that, so it's always true
        float nearest = Mathf.Infinity;

        for (int i = 0; i < hits.Length; i++)
        {
            // only change the nearest distance if the next one is smaller than the last one
            if (hits[i].distance < nearest && (!hits[i].collider.isTrigger) &&
                hits[i].collider.attachedRigidbody.gameObject != self)
            {
                // Changes the nearest distance to the smaller distance
                nearest = hits[i].distance;
                targetDist = -transform.InverseTransformPoint(hits[i].point).z;
                hitSomething = true;
            }
        }

        // For debugging purposes in the editor
        if (hitSomething)
        {
            Debug.DrawRay(ray.origin, -transform.forward * (targetDist + sphereCastRadius), Color.red);
        }

        // hit something so the camera moves to a better position
        protecting = hitSomething;
        distance = Mathf.SmoothDamp(distance, targetDist, ref moveVelocity,
            distance > targetDist ? clipSmooth : returnTime);
        distance = Mathf.Clamp(distance, nearClip, defaultDistance);
        offset = -Vector3.forward * distance;     
    }

    public void wallClip()
    {
        ray.origin = transform.position;
        ray.direction = -transform.forward;

        if(Physics.Raycast(ray, out hit, defaultDistance))
        {
            float targetDist = -transform.InverseTransformDirection(hit.point).z;
            distance = Mathf.SmoothDamp(distance, targetDist, ref moveVelocity,
                distance > targetDist ? clipSmooth : returnTime);
            distance = Mathf.Clamp(distance, nearClip, defaultDistance);
            offset = -Vector3.forward * distance;
        }
    }

    public class RayHitComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
        }
    }
}

