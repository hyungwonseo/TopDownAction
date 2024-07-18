using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "GlobalData", menuName = "GameData/GlobalData")]
public class GlobalData : ScriptableObject
{
    public int arrows;
    public int keys;
    public int hp;

    public void LoadFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "GlobalData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
