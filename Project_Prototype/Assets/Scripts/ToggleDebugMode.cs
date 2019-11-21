using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDebugMode : MonoBehaviour
{
    [Header("Debug Mode")]
    [Tooltip("Keep this off to start game normally!")]
    public bool startInDebugMode = false;
    public KeyCode debugStartKey;
    public int debugPlayerCount = 0;
}
