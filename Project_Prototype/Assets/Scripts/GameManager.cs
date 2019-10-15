using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameRoundTimer = 300;

    // Update is called once per frame
    void Update()
    {
        gameRoundTimer -= Time.deltaTime;
        if(gameRoundTimer <= 0)
        {
            SceneManager.LoadScene("Main-Menu");
        }
    }
}
