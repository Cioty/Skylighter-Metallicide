using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    [Header("Properties")]
    public int scoreForMechKill = 1;
    public int scoreForCoreKill = 2;
    public int scoreForEscape = 3;
    public int scoreForDeath = -2;

    // Individual counts.
    private int totalMechKills = 0;
    private int totalCoreKills = 0;
    private int totalEscapes = 0;
    private int totalDeaths = 0;

    // Private properties.
    private int totalScore = 0;

    public void KilledMech()
    {
        totalScore += scoreForMechKill;
        totalMechKills += 1;
    }

    public void KilledCore()
    {
        totalScore += scoreForCoreKill;
        totalCoreKills += 1;
    }

    public void HasEscaped()
    {
        totalScore += scoreForEscape;
        totalEscapes += 1;
    }

    public void HasDied()
    {
        totalScore += scoreForDeath;
        totalDeaths += 1;
    }

    public int TotalMechKills
    {
        get { return totalMechKills; }
    }

    public int TotalCoreKills
    {
        get { return totalCoreKills; }
    }


    public int TotalEscapes
    {
        get { return totalEscapes; }
    }


    public int TotalDeaths
    {
        get { return totalDeaths; }
    }

    public int TotalScore
    {
        get { return totalScore;  }
    }

}
