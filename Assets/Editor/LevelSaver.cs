using UnityEditor;
using UnityEngine;
using System.Collections;

public static class MenuExtender
{
    [MenuItem("Ool/Save Level")]
    private static void OolSaveLevel()
    {
        var window = EditorWindow.GetWindow<LevelSaver>();
        window.Show();
    }

    [MenuItem("Ool/Load Level")]
    private static void OolLoadLevel()
    {
        var window = EditorWindow.GetWindow<LevelLoader>();
        window.Show();
    }
}

public class LevelSaver : EditorWindow
{
    private int levelNumber = 0;

    void OnGUI()
    {
        levelNumber = EditorGUILayout.IntField("Level Number", levelNumber);
        if (GUILayout.Button("Save"))
        {
            LevelFactory.SaveLevel(levelNumber);
            AssetDatabase.Refresh();
            Close();
        }
    }
}

public class LevelLoader : EditorWindow
{
    private int levelNumber = 0;

    void OnGUI()
    {
        levelNumber = EditorGUILayout.IntField("Level Number", levelNumber);
        if (GUILayout.Button("Save"))
        {
            LevelFactory.LoadLevel(levelNumber);
            AssetDatabase.Refresh();
            Close();
        }
    }
}

