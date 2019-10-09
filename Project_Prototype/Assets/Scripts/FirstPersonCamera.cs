using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("References")]
    public GameObject playerObject;
    public PlayerHandler playerHandler;
    public Transform mechHullTransform;
    public Transform mechHipTransform;
    public Transform mechCoreTransform;
    private Quaternion defaultHipRotation;

    [Header("Camera Properties")]
    public float sensitivity = 2.5f;
    public float smoothing = 2.0f;
    public float minX = -360, maxX = 360;
    public float minY = -60, maxY = 80;
    private Vector2 mouseLook, smoothV;

    private void Start()
    {
        // Setting up the default hip transform. (Current static, will implement direction on move soon).
        this.defaultHipRotation = mechHipTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Getting the mouse delta.
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        // Getting the interpolated result between the two float values.
        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);

        // Incrementally adding to the camera look.
        mouseLook += smoothV;

        // Locking the mouse Y.
        mouseLook.y = Mathf.Clamp(mouseLook.y, minY, maxY);

        // Restraining the hip transform to prevent rotation, only if the player has no velocity.
        if ((int)(playerHandler.CurrentVelocity.x) == 0 && 
            (int)(playerHandler.CurrentVelocity.z) == 0)
            this.mechHipTransform.rotation = defaultHipRotation;
        else
        {
            this.mechHipTransform.rotation = playerObject.transform.localRotation;
            this.defaultHipRotation = playerObject.transform.localRotation;
        }

        // Applying rotation to the neck transform, and correcting the angle.
        mechCoreTransform.localRotation = (Quaternion.AngleAxis(-mouseLook.y, Vector3.right));

        // Applying rotation to the player transform.
        playerObject.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, playerObject.transform.up);
    }
}
