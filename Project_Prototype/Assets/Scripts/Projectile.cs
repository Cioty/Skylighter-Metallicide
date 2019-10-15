using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple projectile script to control the behaviour of the object after being projected.
public class Projectile : MonoBehaviour
{
    private StateManager playerStateManager;
    public GameObject explosionEffect;
    public float maxTimer = 5.0f;
    private float timer = 0.0f;
    public GameObject playerObject;

    private void Update()
    {
        // Simple timer to destory the object after a certain time.
        timer += Time.deltaTime;

        if (timer > maxTimer)
            Explode();
    }

    public void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        GameObject explosionObject = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionObject, 1.9f);
        Destroy(gameObject);
    }
}
