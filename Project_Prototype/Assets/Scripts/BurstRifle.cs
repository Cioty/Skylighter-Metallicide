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
using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
using System.Collections.Generic;

public class BurstRifle : MonoBehaviour
{
    public PlayerHandler playerHandler;

    public GameObject BurstFireRifle;

    public Camera fpsCamBr;

    public GameObject firePoint2;

    public GameObject bulletPrefab;
    public GameObject bulletParent;
    private List<GameObject> bulletPool = new List<GameObject>();

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

    private void Awake()
    {
        if(bulletPrefab)
        {
            for(int i = 0; i < gunAmmo; ++i)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletParent.transform);
                bullet.SetActive(false);
                bulletPool.Add(bullet);
            }
        }
    }

    void Update()
    {
        if(playerHandler.IsControllable && !playerHandler.IsTestDummy)
        {
            float leftTrigHeight = XCI.GetAxis(XboxAxis.LeftTrigger, playerHandler.AssignedController);
            if (Input.GetButton("Fire2") || leftTrigHeight >= 0.5f && gunCharge == 0.0f)
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

            // if shooting 
            // get the amount of shots taken
            // for those shots taken
            // active & pos bullet
            // translate bullet from origin to dest
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
        Vector3 forwardVector = firePoint2.transform.forward;
        float randX = Random.Range(-randXRange, randXRange);
        float randY = Random.Range(-randYRange, randYRange);
        float randZ = Random.Range(-randZRange, randZRange);
        forwardVector.x += randX;
        forwardVector.y += randY;
        forwardVector.z += randZ;

        Ray ray = new Ray();
        ray.origin = firePoint2.transform.position;
        ray.direction = forwardVector;

        RaycastHit hit;

        bulletPool[gunAmmoShot].SetActive(true);
        bulletPool[gunAmmoShot].GetComponent<BurstRifleBullet>().Setup(playerHandler, firePoint2.transform.position, forwardVector, 100f);
        if (playerHandler.IsControllable)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 0.1f);
            if (Physics.Raycast(ray, out hit, gunRange))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player" + playerHandler.ID) && hit.transform.gameObject.tag != "Player")
                {
                    PlayerHandler otherPlayerHandler = GetComponentInParent<PlayerHandler>();

                    if (otherPlayerHandler.CurrentState == StateManager.PLAYER_STATE.Mech)
                    {
                        Debug.Log("Hit mech!");
                        if (playerHandler.Mech_TakeDamage((int)gunDamage) == 0)
                        {
                            playerHandler.PlayerStats.KilledMech();
                        }
                    }
                    else
                    {
                        // Dealing damage to the player, then checking if their health is 0:
                        if (playerHandler.Core_TakeDamage((int)gunDamage) == 0)
                        {
                            Debug.Log("Hit core!");
                            // Killing the victim:
                            otherPlayerHandler.IsAlive = false;

                            // Adding a kill to the core stats:
                            playerHandler.PlayerStats.KilledCore();
                        }
                    }
                }
            }
        }
    }
}
  