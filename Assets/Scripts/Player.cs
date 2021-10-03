using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float gravity;
    public float dirLerp, jumpHoldTime = 0.1f;

    private World world;
    private Camera cam;
    private Rigidbody rb;
    private PlayerPhysics phys;

    private Vector3 up;
    private Vector2 moveDir;
    private float jumpHoldTimer;
    private bool jump, jumpToggle = true;

    private Vector3 debugVec1, debugVec2;
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, debugVec1);
        Gizmos.DrawRay(transform.position, debugVec2);
    }

    // Start is called before the first frame update
    void Start()
    {
        world = World.instance;
        cam = Camera.main;
        rb = GetComponentInChildren<Rigidbody>();
        phys = GetComponentInChildren<PlayerPhysics>();
    }

    // Update is called once per frame
    void Update()
    {
        FetchInput();
    }

    private void FetchInput()
    {
        bool jumpControl = false;

        Gamepad g = Gamepad.current;
        if(g != null)
        {
            moveDir = g.leftStick.ReadValue();

            jumpControl = g.buttonNorth.isPressed || g.buttonWest.isPressed
                || g.buttonSouth.isPressed || g.buttonEast.isPressed;
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

        jumpControl |= Keyboard.current.spaceKey.isPressed;

        if (!jump)
        {
            if (jumpControl && jumpToggle)
            {
                jump = true;
                jumpHoldTimer = 0;
                jumpToggle = false;
            }
            else if (!jumpToggle && !jumpControl)
                jumpToggle = true;
        }
        else
        {
            jumpHoldTimer += Time.deltaTime;
            if(jumpHoldTimer > jumpHoldTime)
                jump = false;
        }
    }

    private void FixedUpdate()
    {
        up = (rb.position - world.transform.position).normalized;
        
        Move();
        JumpAndFall();
    }

    private void Move()
    {
        // add up/down force relative to camera
        Vector3 forward = cam.transform.forward;
        forward = Vector3.ProjectOnPlane(forward, up);
        forward = forward.normalized * moveDir.y;
        // add left/right force relative to camera
        Vector3 right = cam.transform.right;
        right = Vector3.ProjectOnPlane(right, up);
        right = right.normalized * moveDir.x;

        phys.Move(forward + right, up);
    }

    private void JumpAndFall()
    {
        rb.AddForce(-up * gravity);
        if (jump)
            if (phys.Jump())
            {
                jump = false;
            }
    }
}
