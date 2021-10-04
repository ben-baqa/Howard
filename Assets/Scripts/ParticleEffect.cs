using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    private ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (particles.isStopped)
            Destroy(gameObject);
    }
}
