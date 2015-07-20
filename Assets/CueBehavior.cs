using UnityEngine;
using System.Collections;

public class CueBehavior : MonoBehaviour
{
    private GameworldUIManager gwui;
    private GameManager Manager;
    
    void Start()
    {
        Manager = FindObjectOfType<GameManager>();
        gwui = FindObjectOfType<GameworldUIManager>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Pocket")
        {
             Foul("Scratch!", -100);
        }
        else if (col.gameObject.tag == "OOB")
        {
            Foul("Cue out of bounds!", -50);
        }
    }

    private void Foul(string reason, int penalty)
    {
        Manager.Score += penalty;
        Manager.CurrentFaults++;
        gwui.FoulBall(string.Format("Foul: {0} : {1} point penalty", reason, penalty));
        Destroy(gameObject);
    }
}
