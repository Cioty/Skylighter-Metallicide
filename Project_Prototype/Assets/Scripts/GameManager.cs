/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       GameManager.cs
 * Purpose:     Manages the game time.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Properties")]
    public float gameRoundTimer = 300;

    [Header("References")]
    public GameObject playerManager;
    public GameObject postMatchScoreBoard;
    public GameObject gametimeCanvasObject;
    public TextMeshProUGUI matchTime;

    // Private:
    private bool shouldTime = true;

    // Update is called once per frame
    void Update()
    {
        if(shouldTime)
        {
            gameRoundTimer -= Time.deltaTime;
            if(gameRoundTimer <= 0)
            {
                playerManager.SetActive(false);
                gametimeCanvasObject.SetActive(false);
                postMatchScoreBoard.SetActive(true);
                shouldTime = false;
            }

            if(gameRoundTimer > 60)
                matchTime.text = (gameRoundTimer / 60).ToString("0.00") + " mins";
            else
                matchTime.text = gameRoundTimer.ToString() + " secs";
        }
    }
}
