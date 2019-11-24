/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       FirstPersonCamera.cs
 * Purpose:     The main view of the player in the mech state.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class FirstPersonCamera : MonoBehaviour
{
    private Transform mechObjectTransform;
    private PlayerHandler playerHandler;

    [Header("References")]
    public GameObject playerObject;
    public GameObject mechObject;
    public Transform rightArmSwing;
    public Transform leftArmSwing;
    public Transform mechHipTransform;
    public Transform mechCoreTransform;
    public Transform shieldHipTransform;

    [Header("Hip Transform Properties")]
    public float hipRotationPadding = 0.2f;
    private Quaternion defaultHipRotation = Quaternion.identity;
    private Quaternion lastDirection = Quaternion.identity;
    private Quaternion direction = Quaternion.identity;
    private Quaternion slerpedDirection = Quaternion.identity;
    private Quaternion slerpedDefaultPos = Quaternion.identity;

    [Header("Camera Properties")]
    public float sensitivity = 2.5f;
    public float smoothing = 2.0f;
    public float minX = -360, maxX = 360;
    public float minY = -60, maxY = 80;
    private Vector2 mouseLook, smoothV;
    private float axisX, axisY;

    private void Awake()
    {
        // Getting the required components.
        //playerObject = this.gameObject.transform.parent.gameObject;
        mechObjectTransform = GetComponent<Transform>();
        playerHandler = GetComponentInParent<PlayerHandler>();
    }

    private void Start()
    {
        // Setting up the default hip transform. (Current static, will implement direction on move soon).
        this.defaultHipRotation = mechHipTransform.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        // controller only atm:
        if(playerHandler.AssignedController > 0 && playerHandler.IsControllable)
        {
            axisX = XCI.GetAxis(XboxAxis.RightStickX, playerHandler.AssignedController);
            axisY = XCI.GetAxis(XboxAxis.RightStickY, playerHandler.AssignedController);
        }
        else
        {
            axisX = Input.GetAxisRaw("Mouse X");
            axisY = Input.GetAxisRaw("Mouse Y");
        }

        if(playerHandler.IsControllable)
        {
            // Getting the mouse delta.
            var lookDelta = new Vector2(axisX, axisY);
            lookDelta = Vector2.Scale(lookDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

            // Getting the interpolated result between the two float values.
            smoothV.x = Mathf.Lerp(smoothV.x, lookDelta.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, lookDelta.y, 1f / smoothing);

            // Incrementally adding to the camera look.
            mouseLook += smoothV;

            // Locking the mouse Y.
            mouseLook.y = Mathf.Clamp(mouseLook.y, minY, maxY);

            // Applying rotation to the neck transform, and correcting the angle.
            mechCoreTransform.localRotation = (Quaternion.AngleAxis(-mouseLook.y, Vector3.right));
            rightArmSwing.localRotation = (Quaternion.AngleAxis(-mouseLook.y, Vector3.right));
            leftArmSwing.localRotation = (Quaternion.AngleAxis(-mouseLook.y, Vector3.right));

            // Applying rotation to the player transform.
            mechObjectTransform.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, playerObject.transform.up);

            // Restraining the hip transform to prevent rotation, only if the player has no velocity.
            if (playerHandler.CurrentVelocity.magnitude > hipRotationPadding)
            {
                if (playerHandler.MechController.GetMoveVector() != Vector3.zero)
                {
                    direction = Quaternion.LookRotation(playerHandler.MechController.GetMoveVector(), Vector3.up);
                }

                mechHipTransform.rotation = direction;
                shieldHipTransform.rotation = direction;
                defaultHipRotation = mechHipTransform.rotation;
            }
            else
            {
                mechHipTransform.rotation = defaultHipRotation;
                shieldHipTransform.rotation = defaultHipRotation;
            }
        }
    }

    public Vector2 MouseLook
    {
        get { return mouseLook; }        
    }
}
