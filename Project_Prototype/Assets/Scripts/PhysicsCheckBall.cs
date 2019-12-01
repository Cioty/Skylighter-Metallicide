using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheckBall : MonoBehaviour
{
    public float deathHeight = -30;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <=  deathHeight)
        {
            gameObject.name = "Leak";
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Renderer>().material.color = Color.red;
        }

        //delete balls
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Cleared");
            Destroy(gameObject);
        }
        //freezes balls
        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("Frozen");
        }
    }
}
