/*=============================================================================
 * Game:        Metallicide
 * Version:     Gold
 * 
 * Class:       Burst Rifle.cs
 * Purpose:     Represents a burst rifle
 * 
 * Author:      Daniel Cox
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BurstRifleSpin : MonoBehaviour
{
    public PlayerHandler playerHandler;

    public BurstRifle burstRifle;

    public GameObject barrelSpinning;

    public GameObject bulletBelt;
    public GameObject bulletRing;

    public float barrelAcceleration = 70;
    public float barrelDeceleration = 70;
    public float maxBarrelSpeed = 360;
    public float barrelSpeed;

    public float beltSpeed;
    public float beltAcceleration = 80;
    public float beltDeceleration = 80;
    public float maxBeltSpeed = 360;

    // Update is called once per frame
    void Update()
    {
        float leftTrigHeight = XCI.GetAxis(XboxAxis.LeftTrigger, playerHandler.AssignedController);
        if (Input.GetButton("Fire2") || leftTrigHeight >= 0.5f)
        {
           if (barrelSpeed<maxBarrelSpeed)
           {
               barrelSpeed += barrelAcceleration * Time.deltaTime;
           }
           else
           {
               barrelSpeed = maxBarrelSpeed;
           }

           
        
           if (beltSpeed < maxBeltSpeed)
           {
              beltSpeed += beltAcceleration * Time.deltaTime;
           }
           else
           {
              barrelSpeed = maxBeltSpeed;
           }
        }
       else
        {
            if(barrelSpeed > 0)
            {
                barrelSpeed -= barrelDeceleration * Time.deltaTime;
            } else
            {
                barrelSpeed = 0;
            }
            if (beltSpeed > 0)
            {
                beltSpeed -= beltDeceleration * Time.deltaTime;
            }
            else
            {
                beltSpeed = 0;
            }
        }
       
        barrelSpinning.transform.Rotate(0, 0, barrelSpeed * Time.deltaTime, Space.Self);
        bulletBelt.transform.Rotate(0, 0, beltSpeed * Time.deltaTime, Space.Self);
        bulletRing.transform.Rotate(0, 0, beltSpeed * Time.deltaTime, Space.Self);
    }
}
