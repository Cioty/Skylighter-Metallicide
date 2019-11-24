
using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class BurstRifle : MonoBehaviour
{
    public PlayerHandler playerHandler;

    public GameObject BurstFireRifle;

    public Camera fpsCamBr;

    public GameObject firePoint2;

    //Gun
    public float gunDamage = 10;
    public float gunRange = 100;

    //Gun's Fire
    public int gunAmmo = 30;
    private int gunAmmoShot = 0;
    public float gunCharge = 0.6f;
    public float gunDelay = 0.5f;
    private bool isGunShooting = false;

    //Gun Spread //Shooting function
    public float randXRange = 0.1f;
    public float randYRange = 0.1f;
    public float randZRange = 0.1f;
    public float halfAngle = 0.5f;
    //public float delay = 0.5f;

    void Update()
    {
        float leftTrigHeight = XCI.GetAxis(XboxAxis.LeftTrigger, playerHandler.AssignedController);
        if (Input.GetButton("Fire2") || leftTrigHeight >= 0.5f && gunCharge == 0.0f && playerHandler.IsControllable)
        {
            isGunShooting = true;
        }
        else
            isGunShooting = false;

        if (isGunShooting)
        {
            gunCharge += Time.deltaTime;
            if (gunCharge > gunDelay)
            {

                gunCharge = gunDelay;

                if (gunAmmoShot < gunAmmo)
                {
                    Shoot();
                    gunAmmoShot++;
                    
                }
                else
                {
                 
                    isGunShooting = false;
                    gunCharge = 0.0f;
                    gunAmmoShot = 0;
                }
            }
        }
        else
        {
            gunCharge = 0;
            gunAmmoShot = 0;
        }
    }

    void Shoot()
    {
        //yield return new WaitForSeconds(delay);

        float rand = Random.Range(-halfAngle, halfAngle) + Random.Range(-halfAngle, halfAngle);
        float rand1 = Random.Range(-halfAngle, halfAngle) + Random.Range(-halfAngle, halfAngle);
        float x = rand;
        float y = rand1;
        Vector2 RandomRangedArea;

        RandomRangedArea = new Vector2(x, y);
        Vector3 forwardVector = fpsCamBr.transform.forward;
        float randX = Random.Range(-randXRange, randXRange);
        float randY = Random.Range(-randYRange, randYRange);
        float randZ = Random.Range(-randZRange, randZRange);
        forwardVector.x += randX;
        forwardVector.y += randY;
        forwardVector.z += randZ;

        Ray ray = new Ray();
        ray.origin = fpsCamBr.transform.position;
        ray.direction = forwardVector;

        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 0.1f);
        if (Physics.Raycast(ray, out hit, gunRange))
        {
            
        }
     
    }
}
  