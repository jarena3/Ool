using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class MainMenuManager : MonoBehaviour
{
    public GameManager Manager;

    public GameObject MainMenuCanvas;
    public GameObject HowToCanvas;
    public GameObject HighScoreCanvas;

    public ToggleGroup MaterialToggleGroup;

    public InputField NameInputField;

    public Blur MainCameraBlur;

    public Text MultiplierText;

    private AudioSource audioSource;

    public Toggle SToggle;
    public Toggle MToggle;

	// Use this for initialization
	void Start ()
	{
	    audioSource = GetComponent<AudioSource>();
	    Manager.PlaySound = SToggle.enabled;
	    Manager.PlayMusic = MToggle.enabled;
	}

    public void PlayerNameSet()
    {
        Manager.PlayerName = NameInputField.text;
    }

    public void MaterialSelect()
    {
        var sel = MaterialToggleGroup.ActiveToggles().First().gameObject.name;

        switch (sel)
        {
            case "LeadMatButton": SetMaterial(LevelOptions.MaterialOptions.Lead);
                break;
            case "SandpaperMatButton": SetMaterial(LevelOptions.MaterialOptions.Sandpaper);
                break;
            case "WoodMatButton": SetMaterial(LevelOptions.MaterialOptions.Wood);
                break;
            case "ClassicMatButton": SetMaterial(LevelOptions.MaterialOptions.Normal);
                break;
            case "MetalMatButton": SetMaterial(LevelOptions.MaterialOptions.Metal);
                break;
            case "IceMatButton": SetMaterial(LevelOptions.MaterialOptions.Ice);
                break;
        }

        UISound();
    }

    public void SetMaterial(LevelOptions.MaterialOptions material)
    {
        var m = (int) material;
        MultiplierText.text = "x" + m;
        Manager.BallMaterialOption = material;
        Manager.SetBallMaterial();
        Manager.ScoreMultiplier = m;
    }

    public void HighScores()
    {
        UISound();

        MainCameraBlur.enabled = true;
        HighScoreCanvas.SetActive(true);
        HowToCanvas.SetActive(false);
    }

    public void HowTo()
    {
        UISound();

        MainCameraBlur.enabled = true;
        HowToCanvas.SetActive(true);
        HighScoreCanvas.SetActive(false);
    }

    public void BackButton()
    {
        UISound();
        MainCameraBlur.enabled = false;
        HighScoreCanvas.SetActive(false);
        HowToCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    public void UISound()
    {
        if (Manager.PlaySound)
        {
            audioSource.Play();
        }
    }

    public void SoundToggle()
    {
        Manager.PlaySound = !Manager.PlaySound;
    }

    public void MusicToggle()
    {
        Manager.PlayMusic = !Manager.PlayMusic;
    }
	
}
