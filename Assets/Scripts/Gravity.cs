using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity;

    private Rigidbody rb;
    private World world;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        world = World.instance;
    }

    private void FixedUpdate()
    {
        Vector3 up = transform.position - world.transform.position;
        rb.AddForce(-up.normalized * gravity);
    }
}
