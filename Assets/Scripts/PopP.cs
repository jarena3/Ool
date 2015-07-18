using UnityEngine;
using System.Collections;

public class PopP : MonoBehaviour
{

    public MeshRenderer PMesh;
    public Light PLight;
    public Light PLight2;
    public Material IronMaterial;

    private bool brighten;

    void Start()
    {
        Invoke("StartPop", 3f);
        Invoke("EndPop", 5f);
    }

    void StartPop()
    {
        brighten = true;
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
        brighten = false;
        PLight.gameObject.SetActive(false);
        PLight2.gameObject.SetActive(false);
        PMesh.material = IronMaterial;
    }
}
