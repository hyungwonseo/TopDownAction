using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json; // Json.NET을 사용하기 위한 네임스페이스
using System.Threading.Tasks;

public class GlobalData
{
    public int arrows;
    public int keys;
    public int hp;
    public GlobalData()
    {
        arrows = 0;
        keys = 0;
        hp = 0;
    }
}

public class SceneData
{
    public string scene;
    public List<SceneObject> objects;
    public SceneData()
    {
        objects = new List<SceneObject>();
    }
}

public class SceneObject
{
    public string objectName;
    public bool isEnabled;
}

public class SaveLoadManager : MonoBehaviour
{
    private string filePathGlobal;
    private string filePath;
    private float checkInterval = 0.2f;
    private float tempTime = 0;
    public GlobalData globalData = new GlobalData();
    public SceneData sceneData = new SceneData();

    void Start()
    {
        filePathGlobal = Path.Combine(Application.persistentDataPath, "SavaData.json");
        if (File.Exists(filePathGlobal))
        {
            LoadGlobalData();
            PlayerController.hp = globalData.hp;
            ItemKeeper.hasArrows = globalData.arrows;
            ItemKeeper.hasKeys = globalData.keys;
        }
        else
        {
            globalData.hp = PlayerController.hp;
            globalData.arrows = ItemKeeper.hasArrows;
            globalData.keys = ItemKeeper.hasKeys;
            SaveGlobalData();
        }

        filePath = Path.Combine(Application.persistentDataPath, SceneManager.GetActiveScene().name + ".json");
        if (File.Exists(filePath))
        {
            LoadSceneData();            
            foreach (SceneObject obj in sceneData.objects) 
            {
                if (!obj.isEnabled) {
                    GameObject target = GameObject.Find(obj.objectName);
                    if (target != null) 
                    {
                        if (target.GetComponent<ItemBox>() != null) 
                        {
                            target.GetComponent<ItemBox>().isClosed = false;
                        }
                        else
                        {
                            target.SetActive(false);
                        }                        
                    }
                }
            }
        }
        else
        {   
            sceneData.scene = SceneManager.GetActiveScene().name;         
            AddObjectsToPropsData("Enemy");
            AddObjectsToPropsData("Door");
            AddObjectsToPropsData("Item");

            SaveSceneData();
        }
    }

    void Update()
    {
        tempTime += Time.deltaTime; 
        if (tempTime > checkInterval) 
        {
            bool dataChanged = false;
            if (globalData.hp != PlayerController.hp) 
            {
                globalData.hp = PlayerController.hp;
                dataChanged = true;
            }
            if (globalData.arrows != ItemKeeper.hasArrows)
            {
                globalData.arrows = ItemKeeper.hasArrows;
                dataChanged = true;
            }
            if (globalData.keys != ItemKeeper.hasKeys)
            {
                globalData.keys = ItemKeeper.hasKeys;
                dataChanged = true;
            }
            if (dataChanged)
            {
                SaveGlobalData();
            }
            tempTime = 0;
        }        
    }

    private void AddObjectsToPropsData(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            SceneObject sceneObject = new SceneObject
            {
                objectName = obj.name,
                isEnabled = obj.activeSelf
            };
            sceneData.objects.Add(sceneObject);
        }
    }
    
    public void ChangeProps(string propName, bool value) 
    {
        foreach (SceneObject obj in sceneData.objects)
        {
            if (obj.objectName == propName)
            {
                obj.isEnabled = value;
            }
            SaveSceneData();
        }
    }

    public async void SaveGlobalData()
    {
        await SaveGlobalDataAsync();
    }

    public async void SaveSceneData()
    {
        await SaveSceneDataAsync();
    }

    private async Task SaveGlobalDataAsync()
    {
        string jsonData = JsonConvert.SerializeObject(globalData, Formatting.Indented);        
        using (StreamWriter writer = new StreamWriter(filePathGlobal))
        {
            await writer.WriteAsync(jsonData);
        }
        Debug.Log("Data saved to " + filePathGlobal);
    }

    private async Task SaveSceneDataAsync()
    {
        string jsonData = JsonConvert.SerializeObject(sceneData, Formatting.Indented);        
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            await writer.WriteAsync(jsonData);
        }
        Debug.Log("Data saved to " + filePath);
    }

    // public static void SaveData()
    // {
    //     string jsonData = JsonConvert.SerializeObject(propsData, Formatting.Indented);
    //     File.WriteAllText(filePath, jsonData);
    //     Debug.Log("Data saved to " + filePath);
    // }

    public void LoadGlobalData()
    {
        if (File.Exists(filePathGlobal))
        {
            string jsonData = File.ReadAllText(filePathGlobal);
            globalData = JsonConvert.DeserializeObject<GlobalData>(jsonData);
            Debug.Log("Data loaded from " + filePathGlobal);
        }
        else
        {
            Debug.LogWarning("Save file not found");
        }
    }

    public void LoadSceneData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            sceneData = JsonConvert.DeserializeObject<SceneData>(jsonData);
            Debug.Log("Data loaded from " + filePath);
        }
        else
        {
            Debug.LogWarning("Save file not found");
        }
    }
}