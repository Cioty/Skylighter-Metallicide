/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       PostMatchScoreboard.cs
 * Purpose:     Displays a post match scoreboard to all uses before returning to
 *              the main menu.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostMatchScoreboard : MonoBehaviour
{
    public float screenFreezeTimer = 15.0f;
    public GameManager gameManager;
    public List<Scoreboard_PlayerPanel> playerPanels = new List<Scoreboard_PlayerPanel>();
    private bool hasUpdated = false;


    public void Update()
    {
        if(gameManager.gameRoundTimer <= 0 && !hasUpdated)
        {
            List<PlayerHandler> activePlayers = PlayerManager.instance.ActivePlayers;
            for(int i = 0; i < activePlayers.Count; ++i)
            {
                PlayerHandler handler = activePlayers[i];
                Scoreboard_PlayerPanel playerPanel = playerPanels[i];
                playerPanel.AdjustStatValue(0, handler.PlayerStats.TotalMechKills);
                playerPanel.AdjustStatValue(1, handler.PlayerStats.TotalCoreKills);
                playerPanel.AdjustStatValue(2, handler.PlayerStats.TotalEscapes);
                playerPanel.AdjustStatValue(3, handler.PlayerStats.TotalDeaths);
            }

            hasUpdated = true;
        }

        screenFreezeTimer -= Time.deltaTime;
        if (screenFreezeTimer <= 0)
            SceneManager.LoadScene("Main-Menu", LoadSceneMode.Single);
    }
}
