using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string PlayerName = "Some Goon";

    public LevelOptions.MaterialOptions BallMaterialOption = LevelOptions.MaterialOptions.Normal;
    public int ScoreMultiplier = 5;
    public Material BallMaterial;
    public PhysicMaterial BallPhysicMaterial;
    public Material[] MaterialSelections;
    public PhysicMaterial[] PhysicMaterialSelections;
    public LevelFactory Factory;

    private int currentLevel = 0;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        SetBallMaterial();
    }

    public void NewGame()
    {
        Application.LoadLevel(1);
        Invoke("LoadFirstLevel", 0.1f);
    }

    void LoadFirstLevel()
    {
        currentLevel++;
        Factory.LoadLevel(1);
    }

    void NextLevel()
    {
        ClearTable();
        currentLevel++;
        Factory.LoadLevel(currentLevel);
    }

    private void ClearTable()
    {
        var extantBalls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (var b in extantBalls)
        {
            Destroy(b);
        }
    }

    void GameOver()
    {

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
}