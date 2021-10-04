using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float offset = 251, friction = .05f;
    public Vector3 centreOfMassOffset;

    private World world;
    private Rigidbody rb;
    private Vector3 up;

    private bool splash = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        world = World.instance;
        up = transform.position - world.transform.position;
        up = up.normalized;

        rb.centerOfMass += centreOfMassOffset;
    }

    // Update is called once per frame
    void Update()
    {
        up = (rb.position - world.transform.position).normalized;

        if (Vector3.Dot(rb.position - up * offset, up) < 0)
        {
            if (splash)
            {
                splash = false;
                HutSplash.OnSplash(rb.position);
            }
            rb.velocity *= (1 - friction);
            rb.AddForce(((up * offset) - rb.position) * 100 * rb.mass);
        }
        if (rb.position.magnitude > offset + 2)
            splash = true;
    }
}
