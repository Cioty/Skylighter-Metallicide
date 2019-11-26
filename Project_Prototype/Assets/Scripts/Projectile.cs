/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       Projectile.cs
 * Purpose:     Represents a rocket.
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Properties")]
    public int directHitDamage = 20;
    public int splashDamage = 10;
    public float splashDamageRadius = 3f;
    public float lifeLength = 5.0f;
    private float timer = 0.0f;

    [Header("References")]
    public GameObject explosionEffect;
    public ParticleSystem trailEffect;
    public GameObject projectileMesh;
    public Rigidbody rigidBody;

    [Header("Debug Options")]
    public bool showGizmos = true;

    // Player that fires the projectile:
    private PlayerHandler shooterHandler;
    private bool hasExploded = false;
    private List<PlayerHandler> hitPlayers = new List<PlayerHandler>();

    // Assigns the shooter variable and the correct layermask.
    public void Setup(PlayerHandler shooter)
    {
        shooterHandler = shooter;
        gameObject.layer = LayerMask.NameToLayer(shooterHandler.playerViewMask);
    }

    private void FixedUpdate()
    {
        if(!hasExploded)
        {
            // Timer to destory the object after a certain time.
            timer += Time.deltaTime;

            if (timer > lifeLength)
                Explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasExploded)
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.name = "ITS HITTING THIS";
            // Playing particle effect:
            Explode();

            if (other.gameObject.tag == "Player")
            {
                // Handler of the other player.
                PlayerHandler handler = other.gameObject.GetComponentInParent<PlayerHandler>();

                // Check what state the player is in:
                if (handler.CurrentState == StateManager.PLAYER_STATE.Mech)
                {
                    // Dealing damage to the player, then checking if their health is 0:
                    if (handler.Mech_TakeDamage(directHitDamage) == 0)
                    {
                        // Adding a mech kill to the player stats:
                        shooterHandler.PlayerStats.KilledMech();
                    }
                }
                else
                {
                    // Dealing damage to the player, then checking if their health is 0:
                    if (handler.Core_TakeDamage(directHitDamage) == 0)
                    {
                        // Killing the victim:
                        handler.IsAlive = false;

                        // Adding a kill to the core stats:
                        shooterHandler.PlayerStats.KilledCore();
                    }
                }
            }

            // Checking for splash damage:
            CheckForSplashDamage();

            // Destroying this object:
            Destroy(this.gameObject, 2f);
        }
    }

    private void CheckForSplashDamage()
    {
        // Clearing the hit players list:
        hitPlayers.Clear();

        // Getting all the colliders within the sphere:
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashDamageRadius);

        // Looping through the hit colliders:
        for(int i = 0; i < hitColliders.Length; ++i)
        {
            GameObject hitObject = hitColliders[i].gameObject;
            if (LayerMask.LayerToName(hitObject.layer).Contains("Player"))
            {
                // Handler of the other player.
                PlayerHandler player = hitObject.GetComponentInParent<PlayerHandler>();

                // Checking if the player is already in the array.
                if (player && player.ID != shooterHandler.ID && !player.AddToSplashCheck)
                {
                    // Toggling the splash check flag in the player:
                    player.AddToSplashCheck = true;

                    // Adding the player to the array.
                    hitPlayers.Add(player);
                }
            }
        }

        foreach(PlayerHandler handler in hitPlayers)
        {
            // Dealing damage to the player, then checking if their health is 0:
            //Debug.Log("Dealing splash damage to player" + handler.ID);
            if(handler.IsInMech())
            {
                if (handler.Mech_TakeDamage(splashDamage) == 0)
                {
                    // Adding a mech kill to the player stats:
                    shooterHandler.PlayerStats.KilledMech();
                }
            }
            else
            {
                if (handler.Core_TakeDamage(splashDamage) == 0)
                {
                    // Adding a core kill to the player stats:
                    shooterHandler.PlayerStats.KilledCore();
                }
            }
            handler.AddToSplashCheck = false;
        }
    }

    private void Explode()
    {   
        // Create particle effect at hit position:
        GameObject explosionObject = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionObject, 1.9f);
        this.rigidBody.velocity = Vector3.zero;
        projectileMesh.SetActive(false);

        trailEffect.Stop(); // Stopping the particle effect here?

        hasExploded = true;
    }

    void OnDrawGizmos()
    {
        if(showGizmos)
        {
            // Draw a sphere at the transform's position to show splash damage radius.
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, splashDamageRadius);
        }
    }

    public Rigidbody RigidBody
    {
        get { return rigidBody; }
    }
}
