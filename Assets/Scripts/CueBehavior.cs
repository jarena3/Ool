using UnityEngine;
using System.Collections;

public class CueBehavior : MonoBehaviour
{
    private GameworldUIManager gwui;
    private GameManager Manager;

    public AudioClip TableBounceClip;
    public AudioClip BallHitClip;
    public AudioClip CannonClip;

    private AudioSource aSource;
    
    void Start()
    {
        Manager = FindObjectOfType<GameManager>();
        gwui = FindObjectOfType<GameworldUIManager>();
        aSource = GetComponent<AudioSource>();
        aSource.Play();
    }

    void OnCollisionEnter(Collision col)
    {
        switch (col.gameObject.tag)
        {
            case "Ball":
            case "Cue":
                PlaySound(BallHitClip);
                break;
            case "Pocket":
                Foul("Scratch!", -100);
                break;
            case "OOB":
                Foul("Cue out of bounds!", -50);
                break;
        }
    }

    void PlaySound(AudioClip sound)
    {
        if (!Manager.PlaySound) return;
        aSource.clip = sound;
        aSource.Play();
    }

    private void Foul(string reason, int penalty)
    {
        gwui.PlaySound(Manager.BadSinkClip);
        Manager.Score += penalty;
        Manager.CurrentFaults++;
        gwui.FoulBall(string.Format("Foul: {0} : {1} point penalty", reason, penalty));
        Destroy(gameObject);
    }
}
