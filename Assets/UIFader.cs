using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{

    private Text text;
    private bool fadeout;
    private Color targetColor;

	void Start ()
	{
	    text = GetComponent<Text>();
	    targetColor = text.color;
        text.color = new Color(1,1,1,0);
	    fadeout = false;
        Destroy(gameObject, 4);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (!fadeout)
	    {
	        text.color = Color.Lerp(text.color, targetColor, Time.deltaTime);
	    }

	    if (Math.Abs(text.color.a - 1) < 0.1)
	    {
	        fadeout = true;
	    }

	    if (fadeout)
	    {
            text.color = Color.Lerp(text.color, new Color(0,0,0,0), Time.deltaTime / 2);  
	    }

	}
}
