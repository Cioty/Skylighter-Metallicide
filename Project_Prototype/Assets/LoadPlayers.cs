using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerData.instance != null)
        {
            Vector3 position = Vector3.zero;
            for(int i = 0; i < PlayerData.instance.AssignedPlayers; ++i)
            {
                Instantiate(playerPrefab, position, playerPrefab.transform.rotation);
                position.x += 10;
            }

            // Destroys the kept data.
            Destroy(PlayerData.instance.gameObject);
        }
    }
}
