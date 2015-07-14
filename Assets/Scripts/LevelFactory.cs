using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class LevelFactory : MonoBehaviour
{

    public string datapath;

    void Awake()
    {
        datapath = Application.dataPath + "/Levels/";
    }

    public void LoadLevel(int levelNumber)
    {
        Debug.Log("loading");
        var formatter = new BinaryFormatter();
        FileStream stream = File.OpenRead(datapath + levelNumber + ".lvl");
        var ballList = (BallInfo[]) formatter.Deserialize(stream);
        stream.Close();

        foreach (var ball in ballList)
        {
            var bf = FindObjectOfType<BallFactory>();
            if (bf != null)
            {
                bf.Instantiate(ball);
            }
        }
    }

    public void SaveLevel(int levelNumber)
    {
        var objs = GameObject.FindGameObjectsWithTag("Ball");

        var balls = objs.Select(b => b.GetComponent<Ball>().BallInfo()).ToArray();

        FileStream stream = File.Create(datapath + levelNumber + ".lvl");
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, balls );
        stream.Close();

    }
}