using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameManager Manager;
    public Transform ScoresPanel;
    public GameObject ScorePanelItemPrefab;
    public Text FinalNameText;
    public Text FinalScoreText;
    public Text FinalTimeText;


    void Start()
    {
        Manager = FindObjectOfType<GameManager>();
        AddLevelScores();
        ShowFinalScore();
    }

    private void ShowFinalScore()
    {
        FinalNameText.text = Manager.PlayerName;
        FinalScoreText.text = Manager.Score.ToString();
        FinalTimeText.text = Manager.GameStopwatch.Elapsed.ToString();
    }

    private void AddLevelScores()
    {
        foreach (var s in Manager.Scores)
        {
            CreateScorePanel(s);
        }
    }

    private void CreateScorePanel(GameManager.LevelScore levelScore)
    {
        var spi = Instantiate(ScorePanelItemPrefab);
        spi.GetComponentInChildren<Text>().text = string.Format("Level {0}{3}Score: {1}{3}Faults: {4}{3}{2}",
            levelScore.LevelNumber, levelScore.Score, levelScore.Time.Minutes+"m " + levelScore.Time.Seconds + "s " + levelScore.Time.Milliseconds + "ms", Environment.NewLine, levelScore.Faults);
        spi.transform.SetParent(ScoresPanel);
    }

    public void ReturnToMainMenu()
    {
        Manager.ResetGame();
        FindObjectOfType<ScreenFader>().EndSceneCaller("Menu");
    }
}
