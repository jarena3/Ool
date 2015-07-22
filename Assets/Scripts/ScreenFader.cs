using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {



    public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
    public bool sceneStarting = true;      // Whether or not the scene is still fading in.

    private Image img;

    
    void Start()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        img = GetComponent<Image>();
    }

    void Update ()
    {
        // If the scene is starting...
        if(sceneStarting)
            // ... call the StartScene function.
            StartScene();
    }
    
    
    void FadeToClear ()
    {
        // Lerp the colour of the texture between itself and transparent.
        img.color = Color.Lerp(img.color, Color.clear, fadeSpeed * Time.deltaTime);
    }
    
    
    void FadeToBlack ()
    {
        // Lerp the colour of the texture between itself and black.
        img.color = Color.Lerp(img.color, Color.black, fadeSpeed * Time.deltaTime);
    }
    
    
    void StartScene ()
    {
        // Fade the texture to clear.
        FadeToClear();
        
        // If the texture is almost clear...
        if(img.color.a <= 0.05f)
        {
            // ... set the colour to clear and disable the GUITexture.
            img.color = Color.clear;
            img.enabled = false;
            
            // The scene is no longer starting.
            sceneStarting = false;
        }
    }

    public void EndSceneCaller(string nextLevel)
    {
        StartCoroutine(EndScene(nextLevel));
    }

    private IEnumerator EndScene(string nextLevel)
    {
        img.enabled = true;
        for (int i = 0; i < 50; i++)
        {
            img.color = Color.Lerp(img.color, Color.black, fadeSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Application.LoadLevel(nextLevel);
        sceneStarting = true;
    }
}

