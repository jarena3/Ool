using UnityEngine;
using System.Collections;

public class PopP : MonoBehaviour
{

    public MeshRenderer PMesh;
    public Light PLight;
    public Light PLight2;
    public Material IronMaterial;

    private bool brighten;

    private AudioSource aSource;
    public AudioClip BreakClip;

    void Start()
    {
        aSource = GetComponent<AudioSource>();
        Invoke("StartPop", 3f);
        Invoke("EndPop", 5f);
    }

    void StartPop()
    {
        brighten = true;
        if(FindObjectOfType<GameManager>().PlaySound)
        aSource.Play();
    }

    void Update()
    {
        if (brighten)
        {
            PLight.intensity = Mathf.Lerp(PLight.intensity, 0.7f, Time.deltaTime);
        }
    }

    void EndPop()
    {
        aSource.Stop();
        aSource.clip = BreakClip;
        if (FindObjectOfType<GameManager>().PlaySound)
        {
            aSource.Play();
        }
        brighten = false;
        PLight.gameObject.SetActive(false);
        PLight2.gameObject.SetActive(false);
        PMesh.material = IronMaterial;
    }
}
