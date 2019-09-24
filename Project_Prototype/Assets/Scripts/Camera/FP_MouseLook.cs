using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FP_MouseLook : MonoBehaviour
{

    [Header("Attributes")]
    public GameObject character;
    public Transform pivotTransform;
    public float sensitivity = 2.5f;
    public float smoothing = 2.0f;
    public float minX = -360, maxX = 360;
    public float minY = -60, maxY = 60;
    private Vector2 mouseLook, smoothV;
    private float rotationX, rotationY;
    private Transform startPos, endPos;

    // Start is called before the first frame update
    void Start()
    {
        // Setting the cursors lock state.
        Cursor.lockState = CursorLockMode.Locked;

        this.transform.parent = character.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Updating the mouselook.
        this.UpdateMouseLook();

        // Checking if we want to unlock the mouse.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void UpdateMouseLook()
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
        if (mouseLook.y > maxY)
            mouseLook.y = maxY;
        else if (mouseLook.y < minY)
            mouseLook.y = minY;

        // Applying the movement to the transform.
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

        // Updating the model's waist transform.
        pivotTransform.transform.localRotation = Quaternion.AngleAxis(mouseLook.y, Vector3.up);
    }
}
