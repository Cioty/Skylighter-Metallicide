using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public Transform forceDirection;
    public float launchForce;
    private bool hasLaunched = false;
    private Trigger trigger;
    private Animator animator;


    public void Awake()
    {
       animator = GetComponentInChildren<Animator>();
       trigger = GetComponentInChildren<Trigger>();
    }

    public void FixedUpdate()
    {
        GameObject collidedObject = trigger.CollidedGameObject();
        if (collidedObject && collidedObject.tag == "Player")
        {
            PlayerHandler playerHandler = collidedObject.GetComponentInParent<PlayerHandler>();

            if (playerHandler.IsGrounded)
                hasLaunched = false;

            if (trigger.IsEnabled())
            {
                hasLaunched = true;
                animator.SetTrigger("Jump");
            }

            if (hasLaunched)
                playerHandler.GetCurrentRigidBody().AddForce((forceDirection.eulerAngles.normalized) * launchForce, ForceMode.Impulse);
        }
    }
}
