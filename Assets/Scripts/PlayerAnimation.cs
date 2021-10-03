using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public float normLerp = .2f, speedScale;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump()
    {
        anim.SetTrigger("jump");
    }

    public void SetWater(bool b)
    {
        anim.SetBool("water", b);
    }

    public void Process(Vector3 pos, Vector3 normal,
        Vector3 vel, bool ground)
    {
        transform.position = pos;

        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(
                Vector3.ProjectOnPlane(vel, normal), normal), normLerp);

        anim.SetFloat("speed", vel.magnitude * speedScale);
        anim.SetBool("ground", ground);
    }
}
