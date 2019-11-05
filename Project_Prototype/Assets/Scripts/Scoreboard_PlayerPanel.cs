/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       Scoreboard_PlayerPanel.cs
 * Purpose:     Holds the strings relating to the stats.
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
using UnityEngine.UI;

public class Scoreboard_PlayerPanel : MonoBehaviour
{
    private Text[] statisticStrings = new Text[4];

    // Start is called before the first frame update
    void Start()
    {
        // Dodgy finds here, might need to find a better way:
        statisticStrings[0] = GameObject.Find("mechKills").GetComponent<Text>();
        statisticStrings[1] = GameObject.Find("ballKills").GetComponent<Text>();
        statisticStrings[2] = GameObject.Find("escaped").GetComponent<Text>();
        statisticStrings[3] = GameObject.Find("deaths").GetComponent<Text>();
    }


    // id represents the state you would like to change (0 = mechKills, 1 = ballKills, 2 = escaped, 3 = deaths). 
    public void AdjustStatValue(int id, int value)
    {
        switch(id)
        {
            case 0:
                statisticStrings[0].text = "Mech Kills: " + value;
                break;
                
            case 1:
                statisticStrings[1].text = "Ball Kills: " + value;
                break;

            case 2:
                statisticStrings[2].text = "Escaped: " + value;
                break;

            case 3:
                statisticStrings[3].text = "Deaths: " + value;
                break;
        }
    }
}
