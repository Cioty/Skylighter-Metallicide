using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair_platform : MonoBehaviour
{
    public GameObject repairKit;
    private Repair_Kit timer;

    // Update is called once per frame
    private void Awake()
    {
        timer = GetComponentInChildren<Repair_Kit>();
    }
}
