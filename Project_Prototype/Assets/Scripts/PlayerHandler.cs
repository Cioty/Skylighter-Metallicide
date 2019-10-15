using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerHandler : MonoBehaviour
{
    public int health;
    public Camera firstPersonCamera;
    private XboxController assignedController;
    private Vector3 currentVelocity = Vector3.zero;

    public int Health {
        get { return health; }
        set { health = value;}
    }

    public Camera FirstPersonCamera
    {
        get { return firstPersonCamera; }
    }

    public Vector3 CurrentVelocity
    {
        get { return currentVelocity; }
        set { currentVelocity = value; }
    }

    public GameObject GameObject
    {
        get { return this.gameObject; }
    }

    public XboxController AssignedController
    {
        get { return assignedController; }
        set { assignedController = value; }
    }

}
