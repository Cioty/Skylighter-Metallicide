using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple projectile script to control the behaviour of the object after being projected.
public class Projectile : MonoBehaviour
{
    private StateManager playerStateManager;
    public GameObject explosionEffect;
    public int damage = 25;
    public float maxTimer = 5.0f;
    private float timer = 0.0f;
    private GameObject playerObject;
    private Rigidbody rb;
    private Trigger trigger;

    private void Awake()
    {
        rb = this.GetComponentInChildren<Rigidbody>();
        trigger = this.GetComponentInChildren<Trigger>();
    }

    public void SetupPlayerObject(GameObject player)
    {
        playerObject = player;
    }

    private void FixedUpdate()
    {
        // Simple timer to destory the object after a certain time.
        timer += Time.deltaTime;

        if (timer > maxTimer)
            Explode();

        if(trigger.CollidedGameObject())
        {
            if(trigger.CollidedGameObject().tag == "Player")
            {
                trigger.CollidedGameObject().GetComponentInParent<PlayerHandler>().MechHealth -= damage;
            }
            Explode();
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
        get { return rb; }
    }

}
