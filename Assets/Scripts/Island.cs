using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public float maxAngle = 30;
    public float friction, centreForce, tiltMoveForce;

    private World world;
    private Rigidbody rb;

    private Vector3 normal;
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        world = World.instance;
        normal = transform.position - world.transform.position;
        offset = normal.magnitude;
        normal = normal.normalized;
    }

    private void FixedUpdate()
    {
        normal = (rb.position - world.transform.position).normalized;
        
        rb.velocity *= (1 - friction);
        rb.AddForce(((normal * offset) - rb.position) * centreForce);

        Vector3 diff = transform.up - normal;
        diff = Vector3.ProjectOnPlane(diff, normal);
        rb.AddForce(diff * tiltMoveForce);
    }
}
