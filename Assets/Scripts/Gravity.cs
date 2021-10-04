using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity;

    private Rigidbody rb;
    private World world;
    private Vector3 up;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        world = World.instance;
    }

    private void FixedUpdate()
    {
        up = transform.position - world.transform.position;
        rb.AddForce(-up.normalized * gravity);
    }
}
