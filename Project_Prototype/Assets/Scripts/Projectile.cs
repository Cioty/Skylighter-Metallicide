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
        if (other.gameObject.tag == "Player")
        {
            // Handler of the other player.
            PlayerHandler handler = other.gameObject.GetComponentInParent<PlayerHandler>();

            // Check what state the player is in:
            if(handler.CurrentState == StateManager.PLAYER_STATE.Mech && !handler.IsInvulnerable)
            {
                // Dealing damage to the player, then checking if their health is 0:
                if(handler.Mech_TakeDamage(directHitDamage) == 0)
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

        // If the projectile hasn't exploded yet, then explode:
        if (!hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    private void Explode()
    {
        // Create particle effect at hit position:
        GameObject explosionObject = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionObject, 1.9f);

        // Checking for splash damage:
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashDamageRadius, gameObject.layer);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.tag == "Player")
            {
                // Handler of the other player.
                PlayerHandler handler = hitColliders[i].gameObject.GetComponentInParent<PlayerHandler>();

                // Making that mech take damage.
                handler.Mech_TakeDamage(splashDamage);
            }
            i++;
        }

        // Last:
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, splashDamageRadius);
    }

    public Rigidbody RigidBody
    {
        get { return rigidBody; }
    }
}
