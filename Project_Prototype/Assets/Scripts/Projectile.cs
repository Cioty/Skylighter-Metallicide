using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple projectile script to control the behaviour of the object after being projected.
public class Projectile : MonoBehaviour
{
    private float timer = 0.0f;
    public float maxTimer = 5.0f;

    public GameObject explosionEffect;
    private StateManager playerStateManager;

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

        // Destorying the target cubes.
        if (collision.gameObject.tag == "TargetCube")
            Destroy(collision.gameObject);
        else if (collision.gameObject.tag == "OtherMech")
        {
            playerStateManager = collision.gameObject.GetComponentInParent<StateManager>();
            playerStateManager.SetState(StateManager.PLAYER_STATE.Ball);
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        GameObject explosionObject = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionObject, 1.9f);
    }
}
