using UnityEngine;
using XboxCtrlrInput;

public class PlayerContainer : MonoBehaviour
{
    public GameObject mechModel;
    public GameObject aButton;
    public GameObject bButton;
    public Animator animator;
    private bool isActive = false;

    private int id;
    public int ID { get { return id; } set { id = value; } }

    private XboxController controller;
    public XboxController Controller { get { return controller; } set { controller = value; } }

    private bool hasPlayer = false;
    public bool HasPlayer { get { return hasPlayer; } set { hasPlayer = value; } }

    public void ToggleMech()
    {
        isActive = !isActive;
        animator.SetBool("DoorOpen", isActive);

        if(isActive)
        {
            aButton.SetActive(false);
            bButton.SetActive(true);
        }
        else
        {
            aButton.SetActive(true);
            bButton.SetActive(false);
        }
    }

    
}
