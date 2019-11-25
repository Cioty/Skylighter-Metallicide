using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair_Kit : MonoBehaviour
{
    private PlayerHandler playerHandler;

    [Header("Animator")]
    public Animator animator;

    LoadPlayers players;

    // The actual object this script affects
    private Renderer thisBox;

    public bool isInteractable = true;

    public float coolDown = 10.0f;
    private float coolDownCounter;

    public float timerReset = 10.0f;

    private float timer = 0.0f;

    public float bobbing = 0.0005f;

    public float spinSpeed = 2.0f;

    private void Awake()
    {
        thisBox = GetComponent<Renderer>();
        coolDownCounter = timerReset;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject playerCollided = other.gameObject;

        if (playerCollided.gameObject.tag == "Player")
        {
            playerHandler = playerCollided.GetComponentInParent<PlayerHandler>();

            // If the player is a Mech
            if (playerHandler.CurrentState == StateManager.PLAYER_STATE.Mech)
            {
                if (playerHandler.MechHealth >= playerHandler.MaxMechHealth)
                {
                    playerHandler.MechHealth = playerHandler.MaxMechHealth;
                }
                else
                {
                    playerHandler.MechHealth += 10;
                    isInteractable = false;
                }

            }

            // If the player is a core
            if (playerHandler.CurrentState == StateManager.PLAYER_STATE.Core)
            {
                if (playerHandler.CoreHealth >= playerHandler.MaxCoreHealth)
                {
                    playerHandler.CoreHealth = playerHandler.MaxCoreHealth;
                }
                else
                {
                    playerHandler.CoreHealth += 10;
                    isInteractable = false;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteractable)
        {
            if (thisBox.enabled)
                thisBox.enabled = false;
            //animator.Play("Preparing");


            coolDownCounter -= Time.deltaTime;


            if (coolDownCounter <= 0)
            {
                animator.SetTrigger("KitSpawned");
                isInteractable = true;
                coolDownCounter = timerReset;
            }
        }

        if (isInteractable)
        {
            thisBox.enabled = true;
            //animator.Play("Idle");

            timer += Time.deltaTime;

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (Mathf.Sin(timer) * bobbing), transform.localPosition.z);
            transform.localRotation.Set(transform.localRotation.x, transform.localRotation.y + spinSpeed * 1 * Time.deltaTime, transform.localRotation.z, transform.localRotation.w);
        }

        animator.SetFloat("Timer", coolDownCounter);
    }
}
