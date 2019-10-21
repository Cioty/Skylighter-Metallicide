using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public Transform forceDirection;
    public float launchForce;
    private bool hasLaunched = false;
    private CharacterController controller;
    private Trigger trigger;
    private Animator animator;


    public void Awake()
    {
       animator = GetComponentInChildren<Animator>();
       trigger = GetComponentInChildren<Trigger>();
    }

    public void FixedUpdate()
    {
        if(trigger.CollidedGameObject() && trigger.CollidedGameObject().tag == "Player")
        {
            controller = trigger.CollidedGameObject().GetComponentInParent<CharacterController>();

            if (controller.isGrounded)
            {
                hasLaunched = false;
                return;
            }

            if (trigger.IsEnabled())
            {
                hasLaunched = true;
            }

            if (hasLaunched)
            {
                animator.SetTrigger("Jump");
                controller.Move((forceDirection.eulerAngles.normalized) * launchForce * Time.deltaTime);
            }
        }
    }
}
