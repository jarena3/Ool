using UnityEngine;
using System.Collections;

public class SCSizerScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(transform.childCount * 100, rect.rect.height);
	}
	

}
