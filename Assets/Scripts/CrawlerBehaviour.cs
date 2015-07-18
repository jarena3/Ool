using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class CrawlerBehaviour : MonoBehaviour
{

    private Vector3 targetPosition;
    public float height;

    public float PositionUpdateTimer;

    public GameObject Smoker;

    private List<Rigidbody> rigidbodies;
    private Animator[] animators;
    public GameObject[] nonBallChildren;

    public GameObject ball;

    public Light light;

    private Transform ffTransform;


	// Use this for initialization
	void Start ()
	{
	    rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
	    rigidbodies.Remove(GetComponent<Rigidbody>());
	    animators = GetComponentsInChildren<Animator>();
	    foreach (var rb in rigidbodies)
	    {
	        rb.isKinematic = true;
	    }
	    ffTransform = transform.Find("FF_Contents/FF_Pivot/FField");
	    ball = transform.Find("FF_Contents/Ball").gameObject;


        InvokeRepeating("ChangeTarget", PositionUpdateTimer, PositionUpdateTimer);

	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position != targetPosition)
        {
            transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime);
        }
	
	}

    void ChangeTarget()
    {
        targetPosition = new Vector3(Random.Range(-5,5), height , Random.Range(-5,5));
    }


    void OnGUI()
    {
        if (GUILayout.Button("pop pop watch motherfuckers drop"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(ffTransform.gameObject);
        Destroy(light.gameObject);

        foreach (var t in transform.GetComponentsInChildren<Transform>())
        {
            t.DetachChildren();
        }

        transform.DetachChildren();

        foreach (var animator in animators)
        {
            animator.speed = 0;
        }

        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
        }

        rigidbodies.Remove(ball.GetComponent<Rigidbody>());
        
        foreach (var rb in rigidbodies)
        { 
            rb.AddExplosionForce(20, Random.insideUnitSphere * 2, 2f, 1.5f, ForceMode.Impulse);
        }

        ball.GetComponent<Rigidbody>().AddForce(Vector3.up * 2);

        Invoke("FallAway", 1.5f);
        Invoke("Die", 2.5f);
        
    }

    void FallAway()
    {
        foreach (var nonBallChild in nonBallChildren)
        {
            nonBallChild.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void Die()
    {
        foreach (var nonBallChild in nonBallChildren)
        {
            Destroy(nonBallChild);
        }
    }
}
