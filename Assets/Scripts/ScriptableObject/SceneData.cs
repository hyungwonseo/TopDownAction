using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "SceneData", menuName = "GameData/SceneData")]
public class SceneData : ScriptableObject
{
    public string sceneName;
    public List<SceneObject> objects;

    void OnEnable()
    {
        // ScriptableObject가 메모리에 로드될 때 호출됩니다.
        if (objects == null)
        {
            objects = new List<SceneObject>();
        }
    }

    public void LoadFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, sceneName + ".json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}

[System.Serializable]
public class SceneObject
{
    public string objectName;
    public bool isEnabled;
}
