using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public float moveForce = 50, jumpForce = 25, friction = .1f;
    public float bodyOffset, groundthreshold = .6f;

    private Rigidbody rb, island;
    private PlayerAnimation anim;

    private Vector3 up, normal;
    private bool ground;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = transform.parent.GetComponentInChildren<PlayerAnimation>();
    }

    public void Move(Vector3 force, Vector3 up)
    {
        this.up = up;
        GetNormal();
        rb.velocity *= (1 - friction);
        rb.AddForce(force * moveForce);

        Vector3 vel = rb.velocity;
        if (island)
            vel -= island.velocity;
        anim.Process(rb.position - normal * bodyOffset, ground? normal:up,
            vel, ground);
        transform.rotation = anim.transform.rotation;
    }

    public bool Jump()
    {
        if (!ground)
            return false;
        anim.Jump();
        rb.AddForce(up * (island ? jumpForce : jumpForce * 2), ForceMode.Impulse);
        if (island)
            island.AddForceAtPosition(
            -up * jumpForce, rb.position, ForceMode.Impulse);
        ground = false;
        return true;
    }

    private void GetNormal()
    {
        RaycastHit rayHit;
        bool hit = Physics.Raycast(rb.position, -up, out rayHit);
        if(hit && rayHit.distance < groundthreshold)
        {
            if (rayHit.collider.CompareTag("Island"))
            {
                if (!ground)
                {
                    island = rayHit.rigidbody;
                    if (island)
                        island.AddForceAtPosition(
                        -up * jumpForce, rb.position, ForceMode.Impulse);
                }
                anim.SetWater(false);
            }
            else if (rayHit.collider.CompareTag("World"))
            {
                island = null;
                anim.SetWater(true);
            }
            ground = true;
        }
        normal = rayHit.normal;
    }
}
