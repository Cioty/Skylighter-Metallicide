using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // Serialized to show the values in the inspector.
    [SerializeField]
    private int health;

    [SerializeField]
    private int damage;

    [SerializeField]
    private GameObject player;

    private Vector3 currentVelocity = Vector3.zero;

    public void Start()
    {
        player = this.gameObject;
    }
    
    public int Health {
        get { return health; }
        set { health = value;}
    }
    
    public int Damage
    {
        get { return Damage; }
        set { Damage = value; }
    }

    public Vector3 CurrentVelocity
    {
        get { return currentVelocity; }
        set { currentVelocity = value; }
    }

    public GameObject GameObject
    {
        get { return player; }
    }
}
