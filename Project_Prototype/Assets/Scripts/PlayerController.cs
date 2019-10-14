using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int controllerNumber;
    private string horizontalAxis, verticalAxis;
    private string rotationX, rotationY;
    private string primaryFire, secondaryFire;
    private string jump;

    public void AssignController(int number)
    {
        controllerNumber = number;
        horizontalAxis = "J" + controllerNumber + "Horizontal";
        verticalAxis = "J" + controllerNumber + "Vertical";
        rotationX = "J" + controllerNumber + "Rotation X"; // mouse x?
        rotationY = "J" + controllerNumber + "Rotation Y"; // mouse x?
        primaryFire = "J" + controllerNumber + "Primary Fire";
        secondaryFire = "J" + controllerNumber + "Secondary Fire";
        jump = "J" + controllerNumber + "Jump";
    }

    public int ControllerNumber { get { return controllerNumber; } }
    public string HorizontalAxis { get { return horizontalAxis; } }
    public string VerticalAxis { get { return verticalAxis; } }
    public string RotationX {  get { return rotationX; } }
    public string RotationY {  get { return rotationY; } }
    public string PrimaryFire { get { return primaryFire; } }
    public string SecondaryFire { get { return secondaryFire; } }
    public string Junp { get { return jump;  } }
}
