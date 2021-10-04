using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int count;
    public float spawnRadius, centreRadius, vertOffset;
    public float timeUntilWave;
    public float variation;

    private int spawned;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        spawned = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time>timeUntilWave)
        {
            if (spawned < count)
            {
                bool placed = false;
                if (centreRadius > spawnRadius)
                {
                    centreRadius = spawnRadius / 2;
                }
                Vector3 up = (transform.position - World.instance.transform.position).normalized;
                Vector3 anchor = transform.position + 100 * up;
                while (!placed)
                {
                    RaycastHit rayHit;
                    Vector3 pos = anchor + spawnRadius *
                        Vector3.ProjectOnPlane(Random.insideUnitSphere, up);
                    while ((pos - anchor).magnitude < centreRadius)
                        pos = anchor + spawnRadius *
                           Vector3.ProjectOnPlane(Random.insideUnitSphere, up);

                    Physics.Raycast(pos, -up, out rayHit);
                    if (!rayHit.collider.CompareTag("Island"))
                    {
                        continue;
                    }
                    Transform t = Instantiate(prefab, rayHit.point + vertOffset * up,
                        Quaternion.identity, transform).transform;
                    t.GetComponent<Bird>().target = rayHit.point;
                    t.up = rayHit.normal;
                    t.Rotate(rayHit.normal, Random.Range(0, 360));
                    t.localScale = Vector3.one / 0.08f;
                    placed = true;
                }
                spawned++;
            }
            else
            {
                time = 0.0f-Random.Range(0.0f,variation);
                spawned = 0;
            }
        }
    }
}
