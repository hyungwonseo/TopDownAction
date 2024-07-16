using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "GlobalRecord", menuName = "GameData/GlobalRecord")]
public class GlobalRecord : ScriptableObject
{
    public int arrows;
    public int keys;
    public int hp;

    public void LoadFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "globalRecord.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
