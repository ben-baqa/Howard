using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public float jumpForce = 25, moveForce;

    private Rigidbody rb, island;
    private Transform bod;

    private Vector3 up;
    private bool ground;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bod = transform.parent.GetComponentInChildren<Animator>().transform;
    }

    public void Move(Vector3 force, Vector3 up)
    {
        this.up = up;
        rb.AddForce(force);
    }

    public bool Jump()
    {
        if (!ground)
            return false;
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
            island = collision.rigidbody;
            if(island)
                island.AddForceAtPosition(
                -up * jumpForce, rb.position, ForceMode.Impulse);
            ground = true;
        }
    }
}
