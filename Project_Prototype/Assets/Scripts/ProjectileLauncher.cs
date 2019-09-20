using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("Attributes")]
    public Transform projectileStartPoint;
    public GameObject projectilePrefab;
    public GameObject player;
    public Camera firstPersonCamera;
    public float projectileSpeed = 10.0f;
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
        Ray ray = firstPersonCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 direction = new Vector3();

        if(Physics.Raycast(ray, out hit))
        {
            direction = (hit.point - projectileStartPoint.position).normalized;
        }
        else
        {
            direction = ray.direction;
        }

        Quaternion rotation = Quaternion.FromToRotation(projectilePrefab.transform.forward, direction);
        GameObject projectile = Instantiate(projectilePrefab, projectileStartPoint.position, rotation);
        projectile.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed, ForceMode.Impulse);

    }


}

