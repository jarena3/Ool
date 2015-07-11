using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string PlayerName = "Some Goon";

    public LevelOptions.MaterialOptions BallMaterialOption;
    public int ScoreMultiplier;
    public Material BallMaterial;
    public PhysicMaterial BallPhysicMaterial;
    public Material[] MaterialSelections;
    public PhysicMaterial[] PhysicMaterialSelections;
	
    void NewGame()
    {

    }

    void NextLevel()
    {

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