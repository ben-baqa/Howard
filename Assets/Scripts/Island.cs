using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public float maxAngle = 30;
    public float friction, centreForce, tiltMoveForce;

    [HideInInspector]
    public float tilt;

    private World world;
    private Rigidbody rb;

    private Vector3 up;
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        world = World.instance;
        up = transform.position - world.transform.position;
        offset = up.magnitude;
        up = up.normalized;
    }

    private void FixedUpdate()
    {
        up = (rb.position - world.transform.position).normalized;
        
        rb.velocity *= (1 - friction);
        rb.AddForce(((up * offset) - rb.position) * centreForce);

        Vector3 diff = transform.up - up;
        tilt = diff.magnitude;
        diff = Vector3.ProjectOnPlane(diff, up);
        rb.AddForce(diff * tiltMoveForce);
    }
}
