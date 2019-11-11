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
    public Rigidbody rigidBody;

    [Header("Debug Options")]
    public bool showGizmos = true;
    public Color color;

    // Player that fires the projectile:
    private PlayerHandler shooterHandler;
    private bool hasExploded = false;

    // Assigns the shooter variable and the correct layermask.
    public void Setup(GameObject shooter)
    {
        shooterHandler = shooter.GetComponent<PlayerHandler>();
        gameObject.layer = LayerMask.NameToLayer(shooterHandler.playerViewMask);
    }

    private void FixedUpdate()
    {
        // Timer to destory the object after a certain time.
        timer += Time.deltaTime;

        if (timer > lifeLength)
            Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!hasExploded)
        {
            if (other.gameObject.tag == "Player")
            {
                // Handler of the other player.
                PlayerHandler handler = other.gameObject.GetComponentInParent<PlayerHandler>();

                // Check what state the player is in:
                if (handler.CurrentState == StateManager.PLAYER_STATE.Mech && !handler.IsInvulnerable)
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
                        // Killing the player:
                        handler.IsAlive = false;

                        // Adjusting the other players score on death:
                        handler.PlayerStats.HasDied();

                        // Adding a kill to the core stats:
                        shooterHandler.PlayerStats.KilledCore();
                    }
                }
            }

            // Explode:
            Explode();
            hasExploded = true;
        }
    }

    private void CheckForSplashDamage()
    {
        // Getting all the colliders within the sphere:
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashDamageRadius); ;
        List<PlayerHandler> hitPlayers = new List<PlayerHandler>();

        int i = 0;
        while (i < hitColliders.Length)
        {
            GameObject hitObject = hitColliders[i].gameObject;
            if (LayerMask.LayerToName(hitObject.layer).Contains("Player"))
            {
                // Handler of the other player.
                PlayerHandler player = hitObject.GetComponentInParent<PlayerHandler>();

                // Checking if the player is already in the array.
                if (!player.HasBeenAddedToSplashCheck)
                {
                    // Adding the player to the array.
                    hitPlayers.Add(player);
                    player.HasBeenAddedToSplashCheck = true;
                }
            }
            i++;
        }

        foreach(PlayerHandler handler in hitPlayers)
        {
            // Making that mech take damage.
            handler.Mech_TakeDamage(splashDamage);
            Debug.Log("Dealing " + splashDamage + " splash damage to player" + handler.ID + "'s mech!");
        }
    }

    private void Explode()
    {
        // Create particle effect at hit position:
        GameObject explosionObject = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionObject, 1.9f);
        
        // Checking for splash damage.
        CheckForSplashDamage();

        // Last:
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a sphere at the transform's position to show splash damage radius.
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, splashDamageRadius);
    }

    public Rigidbody RigidBody
    {
        get { return rigidBody; }
    }
}
