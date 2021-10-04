using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutSplash : MonoBehaviour
{
    private static HutSplash instance;

    public AudioClip[] clips;
    public GameObject splashEffect;

    private AudioSource sfx;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        sfx = GetComponent<AudioSource>();
    }

    public static void OnSplash(Vector3 pos)
    {
        instance.Splash(pos);
    }

    private void Splash(Vector3 pos)
    {
        //particles
        Transform t = Instantiate(splashEffect).transform;
        t.position = pos;
        t.up = pos;

        //sound
        if (sfx.isPlaying)
            return;
        sfx.clip = clips[Random.Range(0, clips.Length)];
        sfx.Play();
    }
}
