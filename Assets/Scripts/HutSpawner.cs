using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int count;
    public float spawnRadius, centreRadius, vertOffset;

    // Start is called before the first frame update
    void Start()
    {
        if(centreRadius > spawnRadius)
        {
            centreRadius = spawnRadius / 2;
        }
        Vector3 up = (transform.position - World.instance.transform.position).normalized;
        Vector3 anchor = transform.position + 100 * up;
        for(int i = 0; i < count; i++)
        {
            RaycastHit rayHit;
            Vector3 pos = anchor + spawnRadius *
                Vector3.ProjectOnPlane(Random.insideUnitSphere, up);
            while((pos - anchor).magnitude < centreRadius)
                pos = anchor + spawnRadius *
                   Vector3.ProjectOnPlane(Random.insideUnitSphere, up);

            Physics.Raycast(pos, -up, out rayHit);
            if (!rayHit.collider.CompareTag("Island"))
            {
                i--;
                continue;
            }

            Transform t = Instantiate(prefab, rayHit.point + vertOffset * up,
                Quaternion.identity, transform).transform;
            t.up = rayHit.normal;
            t.Rotate(rayHit.normal, Random.Range(0, 360));
        }
    }
}
