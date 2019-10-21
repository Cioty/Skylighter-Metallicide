using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private bool triggerEnabled = false;
    private GameObject collidedGameObject;

    private void OnTriggerEnter(Collider other)
    {
        triggerEnabled = true;
        collidedGameObject = other.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        triggerEnabled = true;
        collidedGameObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        triggerEnabled = false;
        collidedGameObject = other.gameObject;
    }

    public bool IsEnabled()
    {
        return triggerEnabled;
    }

    public GameObject CollidedGameObject()
    {
        return collidedGameObject;
    }
}
