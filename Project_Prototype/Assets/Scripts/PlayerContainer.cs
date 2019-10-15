using UnityEngine;
using XboxCtrlrInput;

public class PlayerContainer : MonoBehaviour
{
    private int id;
    public int ID { get { return id; } set { id = value; } }

    private XboxController controller;
    public XboxController Controller { get { return controller; } set { controller = value; } }

    private bool hasPlayer = false;
    public bool HasPlayer { get { return hasPlayer; } set { hasPlayer = value; } }
}
