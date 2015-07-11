using System;
using UnityEngine;
using System.Collections;

public class ViewRotator : MonoBehaviour
{
    private float rotateTargetY;

    void Start()
    {
        rotateTargetY = transform.rotation.y;
    }

    public void RotateRight()
    {
        rotateTargetY = transform.localRotation.eulerAngles.y + 90;

        Debug.Log(rotateTargetY);
    }

    public void RotateLeft()
    {
        rotateTargetY = transform.localRotation.eulerAngles.y - 90;

        Debug.Log(rotateTargetY);
    }

    void FixedUpdate()
    {
        if (Math.Abs(transform.localRotation.eulerAngles.y - rotateTargetY) > 0.1)
        {
            transform.Rotate(Vector3.up, rotateTargetY*Time.deltaTime);
        }
    }

}
