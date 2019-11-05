/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       ButtonManager.cs
 * Purpose:     Manages the button input form the main menu:
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class ButtonManager : MonoBehaviour
{
    public GameObject startButton, exitButton;
    private MoveCamera moveCamera;
    private bool shouldUpdate = true;
    public PTCAssigner ptcAssigner;

    private void Awake()
    {
        moveCamera = Camera.main.GetComponent<MoveCamera>();
    }

    void Update()
    {
        if(shouldUpdate)
        {
            // Checking for controller input.
            if (PTCAssigner.controllerFound)
            {
                if (XCI.GetButton(XboxButton.A, XboxController.First))
                {
                    MoveToConnectControllers();
                }
                else if (XCI.GetButton(XboxButton.B, XboxController.First))
                {
                    Application.Quit();
                }
            }

            // Checking for mouse click.
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == startButton)
                        MoveToConnectControllers();
                    //SceneManager.LoadScene("Map01", LoadSceneMode.Single);

                    else if (hit.collider.gameObject == exitButton)
                        Application.Quit();
                }
            }
        }

        // Setting the should update bool to false on arrival.
        if (moveCamera.HasReachedEnd)
        {
            ptcAssigner.IsAtConnectScreen = true;
            shouldUpdate = false;
        }
    }

    void MoveToConnectControllers()
    {
        moveCamera.StartCameraMove = true;
    }


}
