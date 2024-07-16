using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "SceneRecord", menuName = "GameData/SceneRecord")]
public class SceneRecord : ScriptableObject
{
    public string sceneName;
    public List<SceneEntry> objects;

    public SceneRecord()
    {
        objects = new List<SceneEntry>();
    }

    public void LoadFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, sceneName + "Record.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}

[System.Serializable]
public class SceneEntry
{
    public string entryName;
    public bool isEnabled;
}
