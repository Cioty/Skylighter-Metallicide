using UnityEngine;

// Projectile script to control the behaviour of the object after being projected.
public class Projectile : MonoBehaviour
{
    [Header("Properties")]
    public int damage = 25;
    public float lifeLength = 5.0f;
    private float timer = 0.0f;

    [Header("References")]
    public GameObject explosionEffect;
    public Rigidbody rigidBody;

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
            PlayerHandler handler = other.gameObject.GetComponentInParent<PlayerHandler>();

            if(handler.CurrentState == StateManager.PLAYER_STATE.Mech)
            {
                float mechHealth = handler.mechHealth -= damage;
                Debug.Log(other.gameObject.name + " at " + mechHealth + " health!");
            }
            else
            {
                float coreHealth = handler.coreHealth -= damage;
                Debug.Log(other.gameObject.name + " at " + coreHealth + " health!");
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
