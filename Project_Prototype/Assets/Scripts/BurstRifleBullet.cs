/*=============================================================================
 * Game:        Metallicide
 * Version:     Gold
 * 
 * Class:       Burst Rifle.cs
 * Purpose:     Represents a burst rifle
 * 
 * Author:      Daniel Cox
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using UnityEngine;

public class BurstRifleBullet : MonoBehaviour
{
    private PlayerHandler playerHandler;
    private Vector3 origin;
    private Vector3 direction;
    private float speed;
    private float timer = 0f;
    private float maxTimer = 2f;
    private Vector3 xPos;
    private Vector3 zPos;
    private GameObject PS_Bullet_003;

    public void Setup(PlayerHandler playerHandler, Vector3 origin, Vector3 direction, float speed, float maxTimer = 2f)
    {
        this.playerHandler = playerHandler;
        this.origin = origin;
        this.direction = direction;
        this.speed = speed;
        this.maxTimer = maxTimer;

        this.transform.position = origin;
        transform.forward = direction;
        //this.transform.LookAt(direction.normalized, Vector3.Lerp(xPos, zPos, timer));

    }

    // Update is called once per frame
    void Update()
    {
        if (timer < maxTimer)
        {

            timer += Time.deltaTime;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        else
        {
            timer = 0f;
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // reference the particle system
        // PS_Bullet_003;
        // play on fire

        // stop here and check particles then set unactive
        this.gameObject.SetActive(false);
    }
}
