using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float timer;
    private float maxTimer = 5.0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > maxTimer)
            Destroy(this.gameObject);
    }
}
