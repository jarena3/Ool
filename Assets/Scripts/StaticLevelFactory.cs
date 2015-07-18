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

        for (int i = 0; i < ballList.Length; i++)
        {
            var ball = ballList[i];
            var bf = GameObject.FindObjectOfType<BallFactory>();
            if (bf != null)
            {
                bf.Instantiate(ball,i+1);
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