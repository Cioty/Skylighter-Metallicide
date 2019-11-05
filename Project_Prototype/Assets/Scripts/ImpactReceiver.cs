/*
 * Source: https://answers.unity.com/questions/242648/force-on-character-controller-knockback.html?_ga=2.159461565.1833362836.1572837573-469280801.1572242956
 * Author: hamcav
 */

using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    // Defines the characters mass:
    public float mass = 3f;

    private Vector3 impact = Vector3.zero;
    private CharacterController character;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Applying the impact force:
        if (impact.magnitude > 0.2F)
        {
            character.Move(impact * Time.deltaTime);
        }

        // Consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }
    // Adds a force to the object in passed direction:
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();

        if (dir.y < 0)
            dir.y = -dir.y;

        impact += dir.normalized * force / mass;
    }
}