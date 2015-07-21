using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public int TotalChallengeLevelCount;


    public struct LevelScore
    {
        public int LevelNumber;
        public int Score;
        public int Faults;
        public TimeSpan Time;
    }

    public List<LevelScore> Scores;

    private int cachedScore = 0;
    private TimeSpan cachedTime;

    public string PlayerName = "Some Goon";
    public int Score = 0;

    public Stopwatch GameStopwatch;

    public LevelOptions.MaterialOptions BallMaterialOption = LevelOptions.MaterialOptions.Normal;
    public int ScoreMultiplier = 5;
    public Material BallMaterial;
    public PhysicMaterial BallPhysicMaterial;
    public Material[] MaterialSelections;
    public PhysicMaterial[] PhysicMaterialSelections;
    public LevelFactory Factory;

    public int currentLevel = 0;

    public int CurrentFaults = 0;

    public int activeBallNumber;

    private ScreenFader screenFader;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        SetBallMaterial();
        cachedTime = new TimeSpan(0);
        Scores = new List<LevelScore>();
        screenFader = FindObjectOfType<ScreenFader>();
    }

    public void NewGame()
    {
        GameStopwatch = new Stopwatch();
        screenFader.EndSceneCaller("Game");
        Invoke("LoadFirstLevel", 1f);
    }

    public void LoadFirstLevel()
    {
        currentLevel++;
        activeBallNumber = 1;
        GameStopwatch.Start();
        GetComponent<BallFactory>().TablePivot = GameObject.Find("TablePivot").transform;
        Factory.LoadLevel(1);
    }

    public void NextLevel()
    {
        RecordScore();
        ClearTable();
        currentLevel++;
        activeBallNumber = 1;
        Factory.LoadLevel(currentLevel);
    }

    private void RecordScore()
    {
        Scores.Add(new LevelScore
        {
            LevelNumber = currentLevel,
            Score = Score - cachedScore,
            Faults = CurrentFaults,
            Time = GameStopwatch.Elapsed - cachedTime
        });

        cachedScore = Score;
        cachedTime = GameStopwatch.Elapsed;
        CurrentFaults = 0;
    }

    public void ClearTable()
    {
        var extant = GameObject.FindGameObjectsWithTag("Ball").Concat(GameObject.FindGameObjectsWithTag("Cue"));
        foreach (var go in extant)
        {
            Destroy(go);
        }
    }

    public void GameOver()
    {
        RecordScore();
        GameStopwatch.Stop();
        screenFader.EndSceneCaller("GameOver");
    }

    public void ResetGame()
    {
        Score = 0;
        CurrentFaults = 0;
        currentLevel = 1;
        Scores = new List<LevelScore>();
    }

    public void SetBallMaterial()
    {
        int n = 3;
        switch (BallMaterialOption)
        {
                case LevelOptions.MaterialOptions.Lead: n = 0;
                break;
                case LevelOptions.MaterialOptions.Sandpaper: n = 1;
                break;
                case LevelOptions.MaterialOptions.Wood: n = 2;
                break;
                case LevelOptions.MaterialOptions.Normal: n = 3;
                break;
                case LevelOptions.MaterialOptions.Metal: n = 4;
                break;
                case LevelOptions.MaterialOptions.Ice: n = 5;
                break;
        }

        BallMaterial = MaterialSelections[n];
        BallPhysicMaterial = PhysicMaterialSelections[n];

    }

    public void CheckLevelOver()
    {
        var endOfGame = currentLevel == TotalChallengeLevelCount;

        if (endOfGame)
        {
            FindObjectOfType<GameworldUIManager>().DoGameOverInterstitial();
        }
        else
        {
            FindObjectOfType<GameworldUIManager>().DoLevelInterstitial();
        }
    }


    public void ActivateNextBall()
    {
        activeBallNumber ++;

        var balls = FindObjectsOfType<Ball>().Where(s => s.IsBogus == false).ToList();

        var ordered = balls.OrderBy(s => s.ballNumber);

        if (ordered.FirstOrDefault() != null)
        {
            ordered.First().SetBallAsTarget();
        }
        else
        {
            CheckLevelOver();
        }
 
    }
}