using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHandler playerHandler = other.gameObject.GetComponentInParent<PlayerHandler>();
            playerHandler.RandomSpawn_FromDeath();
        }
    }
}
