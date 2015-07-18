﻿using System;
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

    private int currentLevel = 0;

    public int activeBallNumber;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        SetBallMaterial();
        cachedTime = new TimeSpan(0);
        Scores = new List<LevelScore>();
    }

    public void NewGame()
    {
        GameStopwatch = new Stopwatch();
        Application.LoadLevel(1);
        Invoke("LoadFirstLevel", 0.1f);
    }

    void LoadFirstLevel()
    {
        currentLevel++;
        activeBallNumber = 1;
        GameStopwatch.Start();
        GetComponent<BallFactory>().TablePivot = GameObject.Find("TablePivot").transform;
        Factory.LoadLevel(1);
    }

    void NextLevel()
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
            Time = GameStopwatch.Elapsed - cachedTime
        });

        cachedScore = Score;
        cachedTime = GameStopwatch.Elapsed;
    }

    private void ClearTable()
    {
        var extant = GameObject.FindGameObjectsWithTag("Ball").Concat(GameObject.FindGameObjectsWithTag("Brick"));
        foreach (var go in extant)
        {
            Destroy(go);
        }
    }

    void GameOver()
    {
        RecordScore();
        GameStopwatch.Stop();
        Application.LoadLevel("GameOver");
    }

    public void ResetGame()
    {
        Score = 0;
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
            GameOver();
        }
        else
        {
            NextLevel();
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