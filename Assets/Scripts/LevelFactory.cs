using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public static class LevelFactory
{

    public static string datapath = Application.dataPath + "/Levels/";

    public static void LoadLevel(int levelNumber)
    {
        var formatter = new BinaryFormatter();
        FileStream stream = File.OpenRead(datapath + levelNumber + ".lvl");
        var BallList = (BallInfo[]) formatter.Deserialize(stream);
        stream.Close();

        foreach (var ball in BallList)
        {
            var bf = GameObject.FindObjectOfType<BallFactory>();
            if (bf != null)
            {
                bf.Instantiate(ball);
            }
        }
    }

    public static void SaveLevel(int levelNumber)
    {
        var objs = GameObject.FindGameObjectsWithTag("Ball");

        var balls = objs.Select(b => b.GetComponent<Ball>().BallInfo()).ToArray();

        FileStream stream = File.Create(datapath + levelNumber + ".lvl");
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, balls );
        stream.Close();

    }
}