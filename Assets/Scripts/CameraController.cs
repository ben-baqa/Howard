using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Vector2 mouseV, gPadV;
    public float maxZoom = 5, minZoom = 0.5f;

    private Rigidbody rb;
    private Transform xRot;
    private World world;
    private Camera cam;

    private Vector3 offset, up;
    private float rot = 0, zoom = 1;

    private void Start()
    {
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        cam = Camera.main;
        world = World.instance;
        xRot = transform.GetChild(0);
        offset = cam.transform.localPosition;
    }

    private void Update()
    {
        transform.position = rb.position;
        up = transform.position - world.transform.position;
        transform.up = up;

        Vector2 dir = Vector2.zero;
        Gamepad g = Gamepad.current;
        if(g != null)
        {
            dir = g.rightStick.ReadValue();
            dir.x *= gPadV.x;
            dir.y *= gPadV.y;
        }
        Vector2 mouseDir = Mouse.current.delta.ReadValue();
        mouseDir.x *= mouseV.x;
        mouseDir.y *= mouseV.y;
        dir += mouseDir;

        rot = LoopAngle(rot + dir.x);
        xRot.localEulerAngles = Vector3.up * rot;


        zoom -= Mouse.current.scroll.ReadValue().y / 500f;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.transform.localPosition = offset * zoom;
    }

    private float LoopAngle(float angle, float centre = 180)
    {
        if (angle > centre + 180)
            return LoopAngle(angle - 360);
        else if (angle < centre - 180)
            return LoopAngle(angle + 360);
        return angle;
    }
}
