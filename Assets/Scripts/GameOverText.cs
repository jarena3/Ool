using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{

    public string[] GameOverStrings;


	// Use this for initialization
	void Start ()
	{
	    GetComponent<Text>().text = GameOverStrings[Random.Range(0, GameOverStrings.Length)];
	}
	
}
