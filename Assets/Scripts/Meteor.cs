using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float gravity;

    public Transform target;
    public Vector3 targetOffset;
    public float magnetForce, slamForce, magnetThreshold,
        scaleUp = 2, spin = 300;

    private Rigidbody rb;
    private World world;
    private Vector3 up;
    private bool magnet = false, hit = false, despawn;

    // Start is called before the first frame update
    void Start()
    {
        world = World.instance;
        rb = GetComponent<Rigidbody>();
        rb.AddTorque(transform.up * Random.Range(0, spin) +
            transform.right * Random.Range(0, spin) +
            transform.forward * Random.Range(0, spin));
    }

    private void Update()
    {
        if (!despawn && transform.lossyScale.x < 1)
            transform.localScale *= scaleUp;
    }

    private void FixedUpdate()
    {
        if (despawn)
        {
            transform.localScale *= 0.9f;
            if (transform.lossyScale.x < .01f)
                Destroy(gameObject);
            return;
        }

        up = (transform.position - world.transform.position).normalized;
        rb.AddForce(-up * gravity);
        if (!hit && Vector3.Dot(rb.velocity, up) < magnetThreshold)
            magnet = true;

        if (magnet)
            rb.AddForce(((target.position + targetOffset) - rb.position) * magnetForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hit && collision.collider.CompareTag("Island"))
        {
            magnet = false;
            hit = true;
            Rigidbody island = collision.rigidbody;
        }
        if (collision.collider.CompareTag("World"))
            despawn = true;
    }
}
