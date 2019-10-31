﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameRoundTimer = 300;
    public GameObject playerManager;
    public GameObject postMatchScoreBoard;
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
                postMatchScoreBoard.SetActive(true);
                shouldTime = false;
            }
        }
    }
}
