using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveForce, jumpForce, gravity;
    public float dirLerp;

    private World world;
    private Camera cam;
    private Rigidbody rb;
    private Animator anim;

    private Vector3 normal;
    private Vector2 moveDir;
    private bool jump;

    // Start is called before the first frame update
    void Start()
    {
        world = World.instance;
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FetchInput();
    }

    private void FetchInput()
    {
        Gamepad g = Gamepad.current;
        if(g != null)
        {
            moveDir = g.leftStick.ReadValue();
        }
        else
        {
            Vector2 dir = Vector2.zero;
            Keyboard k = Keyboard.current;
            if (k.dKey.isPressed || k.rightArrowKey.isPressed)
                dir.x = 1;
            if (k.aKey.isPressed || k.leftArrowKey.isPressed)
                dir.x -= 1;
            if (k.wKey.isPressed || k.upArrowKey.isPressed)
                dir.y = 1;
            if (k.sKey.isPressed || k.downArrowKey.isPressed)
                dir.y -= 1;
            if (dir.magnitude > 0)
                dir = dir.normalized;
            moveDir = Vector2.Lerp(moveDir, dir, dirLerp);
        }
    }

    private void FixedUpdate()
    {
        normal = (transform.position - world.transform.position).normalized;

        rb.AddForce(-normal * gravity);
        
        Move();
    }

    private void Move()
    {
        // add up/down force relative to camera
        Vector3 forward = cam.transform.forward;
        forward = Vector3.ProjectOnPlane(forward, normal);
        rb.AddForce(forward * moveDir.y * moveForce);
        // add left/right force relative to camera
        Vector3 right = cam.transform.right;
        right = Vector3.ProjectOnPlane(right, normal);
        rb.AddForce(right * moveDir.x * moveForce);
    }
}
