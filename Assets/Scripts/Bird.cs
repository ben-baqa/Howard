using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    public Vector3 target;
    public float speed;
    public float timeToflyAway;
    public float despawnTime;
    public float fallingGravity;
    public float landedGravity;
    public float variation;

    private Island island;
    private Rigidbody rb;
    private Vector3 relativeTarget;
    private float timer;
    private float timeLimit;
    private bool hasCollided;
    private bool flyAway;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        island = FindObjectOfType<Island>();
        hasCollided = false;
        GetComponent<Gravity>().gravity = fallingGravity;
    }

    void FixedUpdate()
    {
        relativeTarget = island.transform.position + target;
        if(!hasCollided){
            rb.AddForce(Vector3.ProjectOnPlane((relativeTarget-transform.position)*speed, transform.position));
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > timeLimit)
            {
                if (flyAway)
                {
                    Destroy(gameObject);
                }
                else
                {
                    timer = 0.0f;
                    flyAway = true;
                    timeLimit = despawnTime;
                }
            }
            if (flyAway)
            {
                rb.AddForce(transform.position*speed);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        hasCollided = true;
        GetComponent<Gravity>().gravity=landedGravity;
        timer = 0.0f;
        timeLimit = timeToflyAway+Random.Range(0.0f, variation);
    }
}
