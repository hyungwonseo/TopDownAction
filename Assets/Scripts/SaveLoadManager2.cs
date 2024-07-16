using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json; // Json.NET을 사용하기 위한 네임스페이스
using System.Threading.Tasks;

public class SaveLoadManager2 : MonoBehaviour
{
    public GlobalRecord globalRecord;
    public SceneRecord sceneRecord;

    private float checkInterval = 0.2f;
    private float tempTime = 0;
    private string globalRecordPath;
    private string sceneRecordPath;

    void Start()
    {
        globalRecordPath = Path.Combine(Application.persistentDataPath, "globalRecord.json");
        sceneRecordPath = Path.Combine(Application.persistentDataPath, SceneManager.GetActiveScene().name + "Record.json");

        LoadGlobalRecord();
        LoadSceneRecord();        
    }

    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > checkInterval)
        {
            bool dataChanged = false;
            if (globalRecord.hp != PlayerController.hp)
            {
                globalRecord.hp = PlayerController.hp;
                dataChanged = true;
            }
            if (globalRecord.arrows != ItemKeeper.hasArrows)
            {
                globalRecord.arrows = ItemKeeper.hasArrows;
                dataChanged = true;
            }
            if (globalRecord.keys != ItemKeeper.hasKeys)
            {
                globalRecord.keys = ItemKeeper.hasKeys;
                dataChanged = true;
            }
            if (dataChanged)
            {
                SaveGlobalRecord();
            }
            tempTime = 0;
        }
    }

    private void AddObjectsToPropsData(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            SceneEntry sceneEntry = new SceneEntry
            {
                entryName = obj.name,
                isEnabled = obj.activeSelf
            };
            sceneRecord.objects.Add(sceneEntry);
        }
    }

    public void ChangeProps(string propName, bool value)
    {
        foreach (SceneEntry entry in sceneRecord.objects)
        {
            if (entry.entryName == propName)
            {
                entry.isEnabled = value;
                break;
            }
        }
        SaveSceneRecord();
    }

    public void SaveGlobalRecord()
    {
        string json = JsonConvert.SerializeObject(globalRecord, Formatting.Indented);
        File.WriteAllText(globalRecordPath, json);
        Debug.Log("Global record saved.");
    }

    public void SaveSceneRecord()
    {
        string json = JsonConvert.SerializeObject(sceneRecord, Formatting.Indented);
        File.WriteAllText(sceneRecordPath, json);
        Debug.Log("Scene record saved.");
    }

    public void LoadGlobalRecord()
    {
        if (File.Exists(globalRecordPath))
        {
            string json = File.ReadAllText(globalRecordPath);
            globalRecord = JsonConvert.DeserializeObject<GlobalRecord>(json);
            
            PlayerController.hp = globalRecord.hp;
            ItemKeeper.hasArrows = globalRecord.arrows;
            ItemKeeper.hasKeys = globalRecord.keys;
        }
        else
        {
            globalRecord = new GlobalRecord
            {
                hp = PlayerController.hp,
                arrows = ItemKeeper.hasArrows,
                keys = ItemKeeper.hasKeys
            };
            SaveGlobalRecord();
        }
    }

    public void LoadSceneRecord()
    {
        if (File.Exists(sceneRecordPath))
        {
            string json = File.ReadAllText(sceneRecordPath);
            sceneRecord = JsonConvert.DeserializeObject<SceneRecord>(json);

            foreach (SceneEntry entry in sceneRecord.objects)
            {
                GameObject target = GameObject.Find(entry.entryName);
                if (target != null)
                {
                    if (!entry.isEnabled)
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
            sceneRecord = new SceneRecord
            {
                sceneName = SceneManager.GetActiveScene().name,
                objects = new List<SceneEntry>()
            };
            AddObjectsToPropsData("Enemy");
            AddObjectsToPropsData("Door");
            AddObjectsToPropsData("Item");
            SaveSceneRecord();            
        }
    }
}