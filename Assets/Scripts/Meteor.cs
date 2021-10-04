using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float gravity;

    public Transform target;
    public Vector3 targetOffset;
    public float magnetForce, slamForce, magnetThreshold;

    private Rigidbody rb;
    private World world;
    private Vector3 up;
    private bool magnet = false, hit = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        world = World.instance;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        up = transform.position - world.transform.position;
        rb.AddForce(-up.normalized * gravity);
        if (!hit && Vector3.Dot(rb.velocity, up) < magnetThreshold)
            magnet = true;

        if (magnet)
            rb.AddForce(((target.position + targetOffset) - rb.position) * magnetForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Island"))
        {
            magnet = false;
            hit = true;
            Rigidbody island = collision.rigidbody;
            if (island)
                island.AddForceAtPosition(rb.position, -up * slamForce);
        }
    }
}
