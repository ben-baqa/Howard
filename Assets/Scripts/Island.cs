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

        Vector3 euler = Quaternion.FromToRotation(normal, transform.up).eulerAngles,
            right = Vector3.ProjectOnPlane(transform.right, normal),
            forward = Vector3.ProjectOnPlane(transform.forward, normal);
        euler.x = LoopAngle(euler.x, 0);
        euler.z = LoopAngle(euler.z, 0);
        rb.AddForce(-right * euler.z * tiltMoveForce);
        rb.AddForce(forward * euler.x * tiltMoveForce);
    }

    private float LoopAngle(float angle, float centre = 180)
    {
        if (angle > centre + 180)
            return LoopAngle(angle - 360, centre);
        else if (angle < centre - 180)
            return LoopAngle(angle + 360, centre);
        return angle;
    }
}
