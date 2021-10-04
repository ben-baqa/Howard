using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public int count = 5, rerolls = 20;
    public float safetyThreshold = .9f;

    private List<Vector3> spawnVecs = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        spawnVecs.Add(Vector3.up);
        while(count > 0)
        {
            Vector3 dir = Random.onUnitSphere;
            for(int i = 0; i < rerolls; i++)
            {
                if (IsValid(dir))
                {
                    //print("tool many rerolls, quitting");
                    break;
                }
                dir = Random.onUnitSphere;
            }
            Transform t = Instantiate(prefabs[
                Random.Range(0, prefabs.Length)]).transform;
            //t.LookAt(dir);
            t.GetChild(0).position = dir * 250;
            spawnVecs.Add(dir);
            count--;
        }
    }

    private bool IsValid(Vector3 dir)
    {
        foreach(Vector3 v in spawnVecs)
        {
            if (Vector3.Dot(v, dir) > safetyThreshold)
            {
                //print("YOOF");
                return false;
            }
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 v in spawnVecs)
        {
            Gizmos.DrawLine(transform.position,
                transform.position + v * 300);
        }
    }
}
