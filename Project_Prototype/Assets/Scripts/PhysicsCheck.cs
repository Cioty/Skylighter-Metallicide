using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public GameObject physBall;
    public List<GameObject> ballList;
    private bool spawning = false;
    public float spawnRate = 0.1f;
    public int spawnAmount = 4;
    private float spawnTimer;
    public float spawnRange;
    public GameObject ballParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //create balls
        if(Input.GetKeyDown(KeyCode.O) ) 
        {
            spawning = !spawning;
            spawnTimer = spawnRate;
        }

        if(spawning)
        {
            if (spawnTimer <= 0)
            {
                for (int i = 0; i < spawnAmount; i++)
                {
                    BallCreate();
                }
                spawnTimer = spawnRate;
            } else
            {
                spawnTimer -= Time.deltaTime;
            }
        }
    }

    private void BallCreate()
    {
        var spawnPosition = new Vector3(RangeCreator(transform.position.x, spawnRange), transform.position.y, RangeCreator(transform.position.z, spawnRange));
        var ballObject = Instantiate(physBall, spawnPosition, Quaternion.identity, ballParent.transform);
        ballList.Add(ballObject);

        var randX = Random.Range(-0.5f, 0.5f);
        var randZ = Random.Range(-0.5f, 0.5f);


        ballObject.GetComponent<Rigidbody>().AddForce(new Vector3(randX, 0, randZ));
    }

    public float RangeCreator(float inital, float range)
    {
        return inital += Random.Range(-range, range);
    }
}
