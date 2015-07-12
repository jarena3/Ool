using System;
using UnityEngine;
using System.Collections;

public class ViewRotator : MonoBehaviour
{
    private Quaternion rotateTarget;

    void Start()
    {
        rotateTarget = transform.rotation;
    }

    public void RotateRight()
    {
        rotateTarget *= Quaternion.Euler(0, -90, 0);
    }

    public void RotateLeft()
    {
        rotateTarget *= Quaternion.Euler(0, 90, 0);
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateTarget, Time.deltaTime * 5);
    }

}
