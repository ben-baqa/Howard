using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public float jumpForce = 25, friction = .1f;

    private Rigidbody rb, island;
    private PlayerAnimation anim;
    private Transform bod;

    private Vector3 up, normal;
    private float offset;
    private bool ground;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = transform.parent.GetComponentInChildren<PlayerAnimation>();
        offset = GetComponent<SphereCollider>().radius;
        bod = transform.GetChild(0);
    }

    public void Move(Vector3 force, Vector3 up)
    {
        this.up = up;
        rb.velocity *= (1 - friction);
        rb.AddForce(force);
        anim.Process(rb.position - normal * offset, normal, rb.velocity, ground);
        bod.rotation = anim.transform.rotation;
    }

    public bool Jump()
    {
        if (!ground)
            return false;
        anim.Jump();
        rb.AddForce(up * jumpForce, ForceMode.Impulse);
        if (island)
            island.AddForceAtPosition(
            -up * jumpForce, rb.position, ForceMode.Impulse);
        ground = false;
        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Island"))
        {
            normal = collision.contacts[0].normal;
            if (!ground)
            {
                island = collision.rigidbody;
                if (island)
                    island.AddForceAtPosition(
                    -up * jumpForce, rb.position, ForceMode.Impulse);
                ground = true;
            }
            anim.SetWater(false);
        }
        else if (collision.collider.CompareTag("World"))
        {
            normal = collision.contacts[0].normal;
            anim.SetWater(true);
            ground = true;
        }
    }
}
