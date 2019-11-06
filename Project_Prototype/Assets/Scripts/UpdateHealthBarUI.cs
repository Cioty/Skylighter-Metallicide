using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHealthBarUI : MonoBehaviour
{
    public PlayerHandler playerHandler;
    private RectTransform healthBar;

    private void Awake()
    {
        healthBar = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHandler.CurrentState == StateManager.PLAYER_STATE.Mech)
            healthBar.sizeDelta = new Vector2(playerHandler.mechHealth * 2, healthBar.rect.height);
        else
            healthBar.sizeDelta.Set(playerHandler.coreHealth * 2, healthBar.sizeDelta.x);
    }
}
