using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0.2f;
    private float boost;
    private float max_speed;
    private float min_speed;

    private float timer;
    private float timer_max;
    private int refresh;

    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        min_speed = speed;
        max_speed = 2.0f;
        boost = 0.1f;

        refresh = 0;

        timer = 0.0f;
        timer_max = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float h_input = Input.GetAxis("Horizontal");
        float v_input = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) && refresh == 0 && timer < timer_max)
        {
            if (speed < max_speed)
            {
                speed += boost;
                timer += 1.0f;    
            }
        }
        else if (!Input.GetKey(KeyCode.Space) || timer > timer_max)
        {
            if (speed > min_speed)
            {
                speed -= boost;
                timer = 0.0f;
                refresh = 200;
            }
            if (speed < min_speed)
                speed = min_speed;
        }

        if (refresh > 0)
        {
            refresh -= 1;
        }

        Vector3 direction = new Vector3(h_input, 0.0f, v_input).normalized * speed;

        controller.transform.position += direction;
    }

    
}
