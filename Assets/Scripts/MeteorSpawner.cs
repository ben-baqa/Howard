using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject[] meteor;
    public float interval, timerOffset, SpawnVelocity, spawnSkew, recoil;
    public int fireCount = 8;
    public bool fire;

    private Rigidbody parentRb;
    private Transform world;
    private float timer, fireWaitCount;
    private int fireTimer;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerOffset;
        world = World.instance.transform;
        parentRb = GetComponentInParent<Rigidbody>();
        fireWaitCount = fireCount * Random.Range(1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            timer -= interval;
            if (fire)
            {
                if(fireTimer++ > fireCount)
                {
                    fire = false;
                    fireTimer = 0;
                    fireWaitCount = fireCount * Random.Range(1, 2);
                }
            }
            else
            {
                if(fireTimer++ > fireWaitCount)
                {
                    fire = true;
                    fireTimer = 0;
                }
            }
            if(fire)
                SpawnRock();
        }
    }



    private void SpawnRock()
    {
        GameObject instance = Instantiate(meteor[Random.Range(0, meteor.Length)]);
        Transform t = instance.transform;
        t.parent = transform;
        t.localPosition = Vector3.zero;
        t.LookAt(Random.insideUnitSphere);
        t.localScale = Vector3.one;

        Vector3 up = (transform.position - world.position).normalized;
        Vector3 offset = Vector3.ProjectOnPlane(
            Random.insideUnitSphere * 22, up);
        while(offset.magnitude < 10)
            offset = Vector3.ProjectOnPlane(
                Random.insideUnitSphere * 22, up);

        Rigidbody rb = t.GetComponent<Rigidbody>();
        rb.velocity = up * SpawnVelocity +
            offset * spawnSkew;
        Meteor m = instance.GetComponent<Meteor>();
        m.target = transform.parent;

        m.targetOffset = offset;

        parentRb.AddForce(-transform.up * recoil, ForceMode.Impulse);
    }
}
