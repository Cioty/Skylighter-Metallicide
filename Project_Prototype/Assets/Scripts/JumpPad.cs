using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    //public PlayerHandler playerHandler;
    public float Launch;
    public MechController mechController;
    bool islaunched = false;
    //public GameObject gameObject;
    //private CharacterController cc;
    //public Rigidbody Rigidbody;

    //// this function:
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    mechController.Move(Vector3.up * Launch);
    //}

    //public void Awake()
    //{
    //    cc = gameObject.GetComponent<CharacterController>();
    //}



    public void FixedUpdate()
    {
        //if ()
            //islaunched = false;

        if (islaunched)
        {
            mechController.Move(Vector3.up * Launch * Time.deltaTime);
        }
        
        else
        {
            
        }
            
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (islaunched == false)
        {
            int connecteControllers = 1;
            for (int i = 0; i < connecteControllers; ++i)
            {
                string tag = "Player";
                if (other.gameObject.CompareTag(tag))
                {
                    islaunched = true;
                }
            }
        }
    }
    /*private void OnTriggerExit(Collider other)
    {
        if (islaunched)
        {
            string tag = "Player";
            if (other.gameObject.CompareTag(tag))
            {
                islaunched = false;
            }
        }
    }*/
}
