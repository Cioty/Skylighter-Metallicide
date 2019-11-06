/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       Projectile.cs
 * Purpose:     To represent a rocket.
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
    public int damage = 25;
    public float lifeLength = 5.0f;
    private float timer = 0.0f;

    [Header("References")]
    public GameObject explosionEffect;
    public Rigidbody rigidBody;

    // Player that fires the projectile:
    private PlayerHandler shooterHandler;

    // Assigns the shooter variable and the correct layermask.
    public void Setup(GameObject shooter, string layerMask)
    {
        shooterHandler = shooter.GetComponent<PlayerHandler>();
        gameObject.layer = LayerMask.NameToLayer(layerMask);
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
        Explode();

        if(other.gameObject.tag == "Player")
        {
            // Handler of the other player.
            PlayerHandler handler = other.gameObject.GetComponentInParent<PlayerHandler>();

            // Check what state the player is in:
            if(handler.CurrentState == StateManager.PLAYER_STATE.Mech && !handler.IsInvulnerable)
            {
                float mechHealth = handler.mechHealth -= damage;
                ///Debug.Log(other.gameObject.name + " at " + mechHealth + " health!");

                if(mechHealth == 0)
                {
                    // Adding a kill to the player stats:
                    shooterHandler.PlayerStats.KilledMech();
                }
            }
            else
            {
                float coreHealth = handler.coreHealth -= damage;
                ///Debug.Log(other.gameObject.name + " at " + coreHealth + " health!");

                if(coreHealth == 0)
                {
                    handler.IsAlive = false;

                    // Adjusting the other players score on death:
                    handler.PlayerStats.HasDied();

                    // Adding a kill to the core stats:
                    shooterHandler.PlayerStats.KilledCore();
                }
            }
        }
    }

    private void Explode()
    {
        GameObject explosionObject = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionObject, 1.9f);
        Destroy(gameObject);
    }

    public Rigidbody RigidBody
    {
        get { return rigidBody; }
    }

}
