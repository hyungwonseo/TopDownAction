using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    private static string filePath;
    public static PropsData propsData = new PropsData();

    // Start is called before the first frame update
    void Start()
    {
        propsData.scene = SceneManager.GetActiveScene().name;
        filePath = Path.Combine(Application.persistentDataPath,
            propsData.scene + ".json");
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        if (File.Exists(filePath))
        {
            propsData = LoadData();
            for (int i = 0; i < doors.Length; i++)
            {
                if (!propsData.doorList[i])
                {
                    doors[i].SetActive(false);
                }
            }
        }
        else
        {            
            for (int i=0; i<doors.Length; i++)
            {
                propsData.doorList.Add(true);
            }            
            SaveData();
        }            
    }

    public static void SaveData()
    {
        string jsonData = JsonUtility.ToJson(propsData, true);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Data saved to " + filePath);
    }

    public static PropsData LoadData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            PropsData data = JsonUtility.FromJson<PropsData>(jsonData);
            Debug.Log("Data loaded from " + filePath);
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found");
            return null;
        }
    }

    public static void setDoorOpen(int index)
    {
        if (index >= 0 && index < propsData.doorList.Count)
        {
            propsData.doorList[index] = false;
            SaveData();
        }
        else
        {
            Debug.LogWarning("Index out of range");
        }
    }
}
