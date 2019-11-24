using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMechVisibility : MonoBehaviour
{
    public PlayerHandler playerHandler;
    private string playerLayer, playerView;
    private bool isShowing = false;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = "Player" + playerHandler.ID;
        playerView = playerLayer + "View";
    }

    // Turn on the bit using an OR operation:
    public void Show()
    {
        isShowing = true;
        playerHandler.FirstPersonCamera.cullingMask |= 1 << LayerMask.NameToLayer(playerLayer);
        playerHandler.FirstPersonCamera.cullingMask |= 1 << LayerMask.NameToLayer(playerView);
    }
    // Turn off the bit using an AND operation with the complement of the shifted int:
    public void Hide()
    {
        isShowing = false;
        playerHandler.FirstPersonCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(playerLayer));
        playerHandler.FirstPersonCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(playerView));
    }

    // Toggle the bit using a XOR operation:
    public void Toggle()
    {
        isShowing = !isShowing;
        playerHandler.FirstPersonCamera.cullingMask ^= 1 << LayerMask.NameToLayer(playerLayer);
        playerHandler.FirstPersonCamera.cullingMask ^= 1 << LayerMask.NameToLayer(playerView);
    }

    public bool IsShowing
    {
        get { return isShowing; }
    }
}
