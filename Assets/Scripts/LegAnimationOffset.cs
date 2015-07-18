using UnityEngine;
using System.Collections;

public class LegAnimationOffset : MonoBehaviour
{
    public float OffsetTime;

    void Start()
    {
        var anim = GetComponent<Animator>();
        anim.SetFloat("Offset", OffsetTime);
    }
}

