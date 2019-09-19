using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("Attributes")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float firePower = 10.0f;
    public float fireThreshold = 1.0f;

    private bool readyToFire = true;
    private float fireTimer = 0.0f;


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (readyToFire)
            {
                FireProjectile();
                readyToFire = false;
            }
        }

        if(!readyToFire)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireThreshold)
            {
                fireTimer = 0.0f;
                readyToFire = true;
            }
        }
    }

    private void FireProjectile()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation) as GameObject;
        projectileGO.GetComponent<Rigidbody>().AddForce(firePoint.forward * firePower, ForceMode.Impulse);
    }
}

