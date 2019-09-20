using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple projectile script to control the behaviour of the object after being projected.
public class Projectile : MonoBehaviour
{
    private float timer = 0.0f;
    public float maxTimer = 5.0f;

    private void Update()
    {
        // Simple timer to destory the object after a certain time.
        timer += Time.deltaTime;

        if(timer > maxTimer)
            Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Destorying the object.
        Destroy(this.gameObject);

        // Destorying the target cubes.
        if (collision.gameObject.tag == "TargetCube")
            Destroy(collision.gameObject);
    }
}
