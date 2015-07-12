using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class StaticLevelFactory
{

    public static string datapath = Application.dataPath + "/Levels/";

    public static void LoadLevel(int levelNumber)
    {
        Debug.Log("loading");
        var formatter = new BinaryFormatter();
        FileStream stream = File.OpenRead(datapath + levelNumber + ".lvl");
        var ballList = (BallInfo[])formatter.Deserialize(stream);
        stream.Close();

        foreach (var ball in ballList)
        {
            var bf = GameObject.FindObjectOfType<BallFactory>();
            Debug.Log("!!!");
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
        formatter.Serialize(stream, balls);
        stream.Close();

    }
}