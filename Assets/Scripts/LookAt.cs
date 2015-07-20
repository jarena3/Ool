using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    public Transform target;

    public bool isOut;

    public Vector3 inPosition;
    public Vector3 outPosition;

    void Start()
    {
        inPosition = transform.position;
        outPosition = transform.position + Vector3.forward;
    }


	// Update is called once per frame
	void Update () 
    {

	    if (target != null && target.gameObject.activeSelf)
	    {
	        transform.LookAt(target, Vector3.forward);
	    }

        if (isOut && transform.position != outPosition)
	    {
            transform.position = Vector3.Slerp(transform.position, outPosition, Time.deltaTime);
	    }

        if (!isOut && transform.position != inPosition)
        {
            transform.position = Vector3.Slerp(transform.position, inPosition, Time.deltaTime);
        }
	
	}
}
