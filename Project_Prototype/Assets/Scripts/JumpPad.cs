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
            CharacterController controller = collidedObject.GetComponentInParent<CharacterController>();

            if (controller.isGrounded)
                hasLaunched = false;

            if (trigger.IsEnabled())
            {
                hasLaunched = true;
                animator.SetTrigger("Jump");
            }

            if (hasLaunched)
                controller.Move((forceDirection.eulerAngles.normalized) * launchForce * Time.deltaTime);
        }
    }
}
