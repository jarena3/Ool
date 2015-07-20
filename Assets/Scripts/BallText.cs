using UnityEngine;
using System.Collections;

public class BallText : MonoBehaviour
{

    private Transform ballTransform;

	// Use this for initialization
	void Start ()
	{
	    ballTransform = transform.parent;
	    transform.parent.DetachChildren();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (ballTransform == null)
	    {
	        Destroy(this);
	    }
	    else
	    {
            transform.position = ballTransform.position + Vector3.up;
	    }
	}
}
