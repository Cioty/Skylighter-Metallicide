using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameScreen : MonoBehaviour
{
    public GameObject panelUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
            panelUI.SetActive(false);
    }
}
