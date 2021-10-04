using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public float wobbleThreshold, wobble2Threshold, lerp;
    public AudioSource main, steel, wobble, wobble2, volcano, boom;
    
    private Island island;
    private MeteorSpawner rockSpawner;

    // Start is called before the first frame update
    void Start()
    {
        island = FindObjectOfType<Island>();
        rockSpawner = FindObjectOfType<MeteorSpawner>();

        main.volume = 1;
        steel.volume = 1;
        wobble.volume = 0;
        wobble2.volume = 0;
        volcano.volume = 0;
        boom.volume = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        List<AudioSource> on = new List<AudioSource>(),
            off = new List<AudioSource>();
        on.Add(main);
        on.Add(steel);

        off.Add(wobble2);
        if(island.tilt > wobbleThreshold)
            on.Add(wobble);
        else
            off.Add(wobble);

        if (island.tilt > wobble2Threshold)
            on.Add(wobble2);
        else
            off.Add(wobble2);

        if (rockSpawner.fire)
        {
            on.Add(volcano);
            on.Add(boom);
        }else
        {
            off.Add(volcano);
            off.Add(boom);
        }

        LerpSources(on, 1);
        LerpSources(off);
    }

    private void LerpSources(List<AudioSource> ar, float val = 0)
    {
        foreach (AudioSource a in ar)
            a.volume = Mathf.Lerp(a.volume, val, lerp);
    }
}
