using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroHandler : MonoBehaviour
{
    public int FadeLength;
    public float FadeSpeed;

    public RawImage EwImage;
    public Text EwText;
    public RawImage SagdcImage;
    public Text SagdcText;

    public AudioClip EwClip;
    public AudioClip SaClip;

    public AudioSource AudioSource;

    public ScreenFader ScreenFader;

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine("EW_Fadein", 1.0f);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Input.GetMouseButtonUp(0))
	    {
	        Application.LoadLevel("Menu");
	    }
	}

    private IEnumerator EW_Fadein()
    {
        AudioSource.clip = EwClip;
        AudioSource.PlayDelayed(0.5f);
        
        for (int i = 0; i < FadeLength; i++)
        {
            EwImage.color = Color.Lerp(EwImage.color, Color.white, FadeSpeed/FadeLength);
            EwText.color = Color.Lerp(EwImage.color, Color.white, FadeSpeed / FadeLength);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(4);
        StartCoroutine("EW_Fadeout");
    }

    private IEnumerator EW_Fadeout()
    {
        for (int i = 0; i < FadeLength; i++)
        {
            EwImage.color = Color.Lerp(EwImage.color, Color.clear, FadeSpeed / FadeLength);
            EwText.color = Color.Lerp(EwImage.color, Color.clear, FadeSpeed / FadeLength);

            yield return new WaitForEndOfFrame();
        }
        StartCoroutine("SAGDC_Fadein", 0.5f);
    }

    private IEnumerator SAGDC_Fadein()
    {
        AudioSource.clip = SaClip;
        AudioSource.PlayDelayed(0.5f);

        for (int i = 0; i < FadeLength; i++)
        {
            SagdcImage.color = Color.Lerp(SagdcImage.color, Color.white, FadeSpeed / FadeLength);
            SagdcText.color = Color.Lerp(SagdcImage.color, Color.white, FadeSpeed / FadeLength);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3);

        StartCoroutine("SAGDC_Fadeout", 3f);
    }

    private IEnumerator SAGDC_Fadeout()
    {
        for (int i = 0; i < FadeLength; i++)
        {
            SagdcImage.color = Color.Lerp(SagdcImage.color, Color.clear, FadeSpeed / FadeLength);
            SagdcText.color = Color.Lerp(SagdcImage.color, Color.clear, FadeSpeed / FadeLength);

            yield return new WaitForEndOfFrame();
        }
        
        ChangeScene();
    }

    void ChangeScene()
    {
        ScreenFader.EndSceneCaller("Menu");
    }
}
