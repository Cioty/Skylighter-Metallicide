using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class TP_MouseLook : MonoBehaviour
{
    public GameObject player;
    public PlayerHandler playerHandler;

    public float rotationSpeed = 5.0f;
    public float minY = -35.0f, maxY = 60.0f;

    private Vector3 offset;

    Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
    private float mouse_x;
    private float mouse_y;


    private void Awake()
    {
        offset = transform.position - player.transform.position;
        Cursor.lockState = CursorLockMode.Locked;
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
        transform.LookAt(player.transform.position);
    }

    // Purpose: Handles the X & Y axis rotation for camera orbit around the player
    void Mouse_aiming()
    {
        // controller only atm:
        if (playerHandler.AssignedController > 0)
        {
            mouse_x += XCI.GetAxis(XboxAxis.RightStickX, playerHandler.AssignedController) * rotationSpeed;
            mouse_y += XCI.GetAxis(XboxAxis.RightStickY, playerHandler.AssignedController) * rotationSpeed;
        }
        else
        {
            mouse_x += Input.GetAxis("Mouse X") * rotationSpeed;
            mouse_y += Input.GetAxis("Mouse Y") * rotationSpeed;
        }

        // This keeps the Y-rotation between that angles to prevent full 360 degrees orbit in the Y-Axis
        mouse_y = Mathf.Clamp(mouse_y, minY, maxY);

        Quaternion rotation = Quaternion.Euler(-mouse_y, mouse_x, 0.0f);
        Vector3 position = rotation * offset + player.transform.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
