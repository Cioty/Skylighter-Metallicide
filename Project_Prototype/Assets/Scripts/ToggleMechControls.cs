/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       ToggleMechControls.cs
 * Purpose:     Toggles the mech controls. (Used in debug mode)
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMechControls : MonoBehaviour
{
    private PlayerHandler playerHandler;
    private MechController mechController;
    private FirstPersonCamera firstPersonCamera;
    private ProjectileLauncher projectileLauncher;
    private Dashing dashing;
    private ViewmodelControl viewModelControl;

    void Awake()
    {
        playerHandler = GetComponentInParent<PlayerHandler>();
        mechController = GetComponent<MechController>();
        firstPersonCamera = GetComponent<FirstPersonCamera>();
        projectileLauncher = GetComponent<ProjectileLauncher>();
        dashing = GetComponent<Dashing>();
        viewModelControl = GetComponent<ViewmodelControl>();
    }

    private void Start()
    {
        if (!playerHandler.IsControllable)
        {
            //Debug.Log("Switching off player " + playerHandler.ID +"'s mech control components.");
            //mechController.enabled = false;
            //firstPersonCamera.enabled = false;
            //projectileLauncher.enabled = false;
            //dashing.enabled = false;
            //viewModelControl.enabled = false;
        }
    }
}
