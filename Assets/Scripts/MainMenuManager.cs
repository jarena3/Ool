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

	// Use this for initialization
	void Start () 
    {
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
        MainCameraBlur.enabled = true;
        HighScoreCanvas.SetActive(true);
        HowToCanvas.SetActive(false);
    }

    public void HowTo()
    {
        MainCameraBlur.enabled = true;
        HowToCanvas.SetActive(true);
        HighScoreCanvas.SetActive(false);
    }

    public void BackButton()
    {
        MainCameraBlur.enabled = false;
        HighScoreCanvas.SetActive(false);
        HowToCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }
	
}
