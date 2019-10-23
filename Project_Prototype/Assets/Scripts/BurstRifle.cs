﻿
using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class BurstRifle : MonoBehaviour
{
    public PlayerHandler playerHandler; 

    public ParticleSystem BurstFireRifle;
    public float damage = 10f;
    public float range = 100f;
    //public float halfDeviation = 10f;
    public float randXRange = 0.1f;
    public float randYRange = 0.1f;
    public float randZRange = 0.1f;
    public float halfAngle = 1f;

    public  int  burstBullets = 30;
    private int  bulletsShot = 0;
    public float charge = 0f;
    public float delay = 0.6f;
    private bool isShooting = false;

    //Burst rifle Charger
    public float burstCharge = 0f;
    public float burstDelay = 0.6f;
    public float burstDuration = 0.5f;

    public Camera fpsCambr;

    private IEnumerator firingCoroutine;

    private void StartShooting()
    {
        firingCoroutine = Shoot();
        StartCoroutine(firingCoroutine);
    }

    private void StopShooting()
    {
        firingCoroutine = Shoot();
        StopCoroutine(firingCoroutine);

    }
    // Update is called once per frame
    void Update()
    {
        float leftTrigHeight = XCI.GetAxis(XboxAxis.LeftTrigger, playerHandler.AssignedController);
        if (Input.GetButton("Fire2") || leftTrigHeight >= 0.5f && burstCharge == 0.0f)
        {
            isShooting = true;
        }

        if(isShooting)
        {
            burstCharge += Time.deltaTime;
            if (burstCharge > burstDelay)
            {
                if (bulletsShot < burstBullets)
                {
                    StartCoroutine(Shoot());
                    bulletsShot++;
                }
                else
                {
                    isShooting = false;
                    burstCharge = 0.0f;
                    bulletsShot = 0;
                }
            }
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(delay);

        float rand = Random.Range(-halfAngle, halfAngle) + Random.Range(-halfAngle, halfAngle);
        float rand1 = Random.Range(-halfAngle, halfAngle) + Random.Range(-halfAngle, halfAngle);
        float x = rand;
        float y = rand1;
        Vector2 RandomRangedArea;
    
        RandomRangedArea = new Vector2(x, y);
        Vector3 forwardVector = fpsCambr.transform.forward;
        float randX = Random.Range(-randXRange, randXRange);
        float randY = Random.Range(-randYRange, randYRange);
        float randZ = Random.Range(-randZRange, randZRange);
        forwardVector.x += randX;
        forwardVector.y += randY;
        forwardVector.z += randZ;
       
        Ray ray = new Ray();
        ray.origin = fpsCambr.transform.position;
        ray.direction = forwardVector;
       
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 1.0f);
        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
        yield break;
    }
  
}   
    /// Use of Quaternions
            
            //float deviation = Random.Range(0f, halfDeviation);
            //float Angle = Random.Range(0f, 360f);
            //forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
            //forwardVector = Quaternion.AngleAxis(Angle, Vector3.forward) * forwardVector;
            //forwardVector = fpsCambr.transform.rotation * forwardVector;

    ///Timer
            
            //for (int i = 0; i > 0; i++)
            //{
            //    for (int j = burstBullets; i < 0; i++)
            //    {
            //        yield return new WaitForSeconds(burstTime / burstBullets);
            //    }
            //}
  