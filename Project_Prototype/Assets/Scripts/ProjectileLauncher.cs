/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       ProjectileLauncher.cs
 * Purpose:     Represents a rocket launcher.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using Assets.MultiAudioListener;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class ProjectileLauncher : MonoBehaviour
{
    private GameObject playerObject;
    public PlayerHandler playerHandler;
    private bool readyToFire = true;
    private float fireTimer = 0.0f;
    private const float MAX_TRG_SCL = 1.21f;

    public GameObject rocketLauncherMesh_mech;
    public GameObject rocketLauncherMesh_view;
    private Animator RLAnimator_mech;
    private Animator RLAnimator_view;

    //public AudioSource SFX_RocketFire;
    public MultiAudioSource SFX_RocketFire;

    [Header("Attributes")]
    public Camera firstPersonCamera;
    public Transform projectileStartPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float fireThreshold = 1.0f;

    private void Awake()
    {
        playerObject = this.gameObject.transform.parent.gameObject;
        RLAnimator_mech = rocketLauncherMesh_mech.GetComponent<Animator>();
        RLAnimator_view = rocketLauncherMesh_view.GetComponent<Animator>();
    }

    private void Update()
    {
        float rightTrigHeight = XCI.GetAxis(XboxAxis.RightTrigger, playerHandler.AssignedController);
        if (Input.GetMouseButtonDown(0) || rightTrigHeight >= 0.5f && playerHandler.IsControllable)
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
        RLAnimator_mech.SetTrigger("Fire");
        RLAnimator_view.SetTrigger("Fire");
        //SFX_RocketFire.Play();

        Ray ray = firstPersonCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 direction = Vector3.zero;

        if(Physics.Raycast(ray, out hit))
            direction = (hit.point - projectileStartPoint.position).normalized;
        else
            direction = ray.direction;

        Quaternion rotation = Quaternion.FromToRotation(projectilePrefab.transform.forward, direction);
        Projectile projectile = Instantiate(projectilePrefab, projectileStartPoint.position, rotation).GetComponent<Projectile>();
        projectile.Setup(playerObject);
        projectile.RigidBody.AddForce(direction * projectileSpeed, ForceMode.Impulse);
    }


}

