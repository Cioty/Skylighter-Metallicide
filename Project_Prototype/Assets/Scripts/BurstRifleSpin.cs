using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BurstRifleSpin : MonoBehaviour
{
    public PlayerHandler playerHandler;

    public BurstRifle burstRifle;

    public GameObject barrel_Spinning;

    public GameObject bullet_Belt;

    public float barrelAcceleration = 70;
    public float barrelDeceleration = 70;
    public float maxBarrelSpeed = 360;
    //public float speed2 = 10;
    public float barrelSpeed;
    //public float beltSpeed;

    // Update is called once per frame
    void Update()
    {
        float leftTrigHeight = XCI.GetAxis(XboxAxis.LeftTrigger, playerHandler.AssignedController);
        if (Input.GetButton("Fire2") || leftTrigHeight >= 0.5f)
        {
            //if(zAngle == 1f)
            {
               // zAngle++;
                if (barrelSpeed<maxBarrelSpeed)
                {
                    barrelSpeed += barrelAcceleration * Time.deltaTime;
                } else
                {
                    barrelSpeed = maxBarrelSpeed;
                }

                //zAngle2 += speed2;
                
                //bullet_Belt.transform.Rotate(0, 0, zAngle2, Space.Self);
            }
        } else
        {
            if(barrelSpeed > 0)
            {
                barrelSpeed -= barrelDeceleration * Time.deltaTime;
            } else
            {
                barrelSpeed = 0;
            }
        }

        barrel_Spinning.transform.Rotate(0, 0, barrelSpeed * Time.deltaTime, Space.Self);
    }
}
