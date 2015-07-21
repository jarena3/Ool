using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using Random = UnityEngine.Random;

public class GameworldUIManager : MonoBehaviour
{

    public GameObject PauseMenu;
    public Blur CameraBlur;
    public GameManager Manager;
    public InputManager InManager;
    public string[] GratsTextStrings;
    public Text GratsText;

    public GameObject LevelOverGameObject;
    public Text LevelOverNumber;
    public Text LevelOverScore;
    public Text LevelOverTime;
    public Text LevelOverFaults;

    public GameObject UpdateItemPrefab;
    public GameObject UpdatePanel;

    public GameObject NextButton;
    public GameObject DoneButton;

    void Start()
    {
        Manager = FindObjectOfType<GameManager>();
        CameraBlur.enabled = false;
        ChangeGratsText();
    }

    public void DoGameOverInterstitial()
    {
        DoLevelInterstitial();
        NextButton.SetActive(false);
        DoneButton.SetActive(true);
    }

    public void DoLevelInterstitial()
    {
        InManager.ResetTable();
        Manager.GameStopwatch.Stop();
        Manager.ClearTable();
        CameraBlur.enabled = true;
        LevelOverGameObject.SetActive(true);
        ChangeGratsText();
        var ts = Manager.GameStopwatch.Elapsed;
        var tsString = string.Format("{0}:{1}:{2}", ts.Minutes, ts.Seconds, ts.Milliseconds);
        LevelOverNumber.text = Manager.currentLevel.ToString();
        LevelOverTime.text = string.Format("{0}:{1}{2}", "Total Time", Environment.NewLine, tsString);
        LevelOverScore.text = string.Format("{0}:{1}{2}", "Total Score", Environment.NewLine, Manager.Score);
        LevelOverFaults.text = string.Format("{0}:{1}{2}", "Total Faults", Environment.NewLine, Manager.CurrentFaults);
    }

    public void ChangeGratsText()
    {
        GratsText.text = GratsTextStrings[Random.Range(0, GratsTextStrings.Length)];
    }

    public void NextLevelButton()
    {
        Manager.NextLevel();
        LevelOverGameObject.SetActive(false);
        CameraBlur.enabled = false;
        Manager.GameStopwatch.Start();
    }

    public void GoToGameOver()
    {
        Manager.GameOver();
    }

    public void Pause()
    {
        Manager.GameStopwatch.Stop();
        Time.timeScale = 0;
        CameraBlur.enabled = true;
        PauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        Manager.GameStopwatch.Start();
        Time.timeScale = 1;
        CameraBlur.enabled = false;
        PauseMenu.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 1;
        Manager.ResetGame();
        Application.LoadLevel(0);
    }

    public void SunkBall(string format)
    {
        var uip = Instantiate(UpdateItemPrefab);
        uip.transform.SetParent(UpdatePanel.transform);
        var t = uip.GetComponent<Text>();
        t.text = format;
        t.color = Color.cyan;
    }

    public void FoulBall(string format)
    {
        var uip = Instantiate(UpdateItemPrefab);
        uip.transform.SetParent(UpdatePanel.transform);
        var t = uip.GetComponent<Text>();
        t.text = format;
        t.color = Color.red;
    }
}
