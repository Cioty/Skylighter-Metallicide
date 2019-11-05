/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       ShowCurrentScore.cs
 * Purpose:     Displays the current match time and the users score on the
 *              first person camera.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class ShowCurrentScore : MonoBehaviour
{
    [Header("References")]
    public PlayerHandler handler;
    public GameObject currentScoreUI;
    public GameObject playerHUD;
    public GameManager gameManager;
    public KeyCode keybind;
    public XboxButton buttonBind;
    public Text score;
    public Text timeLeftInMatch;

    // Update is called once per frame
    void Update()
    {
        // If player has an xbox controllor, then handle input with controller buttons:
        if(handler.HasAssignedController)
        {
            if(XCI.GetButtonDown(buttonBind, handler.AssignedController))
            {
                currentScoreUI.SetActive(true);
                playerHUD.SetActive(false);
            }

            if (XCI.GetButtonUp(buttonBind, handler.AssignedController))
            {
                currentScoreUI.SetActive(false);
                playerHUD.SetActive(true);
            }
        }
        else
        {
            // If no xbox controller connected, fallback to keyboard input.
            if (Input.GetKeyDown(keybind))
            {
                currentScoreUI.SetActive(true);
                playerHUD.SetActive(false);
            }

            if (Input.GetKeyUp(keybind))
            {
                currentScoreUI.SetActive(false);
                playerHUD.SetActive(true);
            }
        }


        if(score != null)
        {
            score.text = handler.PlayerStats.TotalScore.ToString();
        }

        if (timeLeftInMatch != null)
        {
            timeLeftInMatch.text = "Time left: " + gameManager.gameRoundTimer.ToString();
        }

    }
}
