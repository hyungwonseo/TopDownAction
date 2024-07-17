using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

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
    string filePathGlobal;
    string filePathScene;
    public GlobalData globalData = new GlobalData();
    public SceneData sceneData = new SceneData();

    void Start()
    {        
        // 글로벌 데이터 저장파일이 있으면 로딩 -> 데이터 업데이트
        // 없으면 -> 코드에 정의된 초기값을 글로벌 데이터에 입력하고 파일로 저장
        // 해당 씬이름의 저장파일이 있으면 로딩 -> 씬의 구성 props의 활성화여부 설정
        // 없으면 -> 씬의 props를 씬 데이터에 적용하고 활성화여부는 true -> 저장
        filePathGlobal = Path.Combine(Application.persistentDataPath
            , "GlobalData.json");
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

        filePathScene = Path.Combine(Application.persistentDataPath
            , SceneManager.GetActiveScene().name + ".json");
        if (File.Exists(filePathScene))
        {
            LoadSceneData();
            foreach (SceneObject obj in sceneData.objects)
            {
                if (!obj.isEnabled) // 비활성화
                {
                    GameObject target = GameObject.Find(obj.objectName);
                    if (target != null) // 해당 아이템이 있으면,
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
            AddObjectToSceneData("Item"); // Tag이름
            AddObjectToSceneData("ItemBox");
            AddObjectToSceneData("Door");
            SaveSceneData();
        }
    }

    void Update()
    {
         
    }    

    public void LoadGlobalData()
    {
        string jsonData = File.ReadAllText(filePathGlobal);
        globalData = JsonConvert.DeserializeObject<GlobalData>(jsonData);
        Debug.Log("Data loaded from " + filePathGlobal);
    }

    public void SaveGlobalData()
    {
        string jsonData = JsonConvert.SerializeObject(globalData
            , Formatting.Indented);
        File.WriteAllText(filePathGlobal, jsonData);
        Debug.Log("Data saved to " + filePathGlobal);
    }

    public void LoadSceneData()
    {
        string jsonData = File.ReadAllText(filePathScene);
        sceneData = JsonConvert.DeserializeObject<SceneData>(jsonData);
        Debug.Log("Data loaded from " + filePathScene);
    }

    public void SaveSceneData()
    {
        string jsonData = JsonConvert.SerializeObject(sceneData
            , Formatting.Indented);
        File.WriteAllText(filePathScene, jsonData);
        Debug.Log("Data saved to " + filePathScene);
    }

    public void AddObjectToSceneData(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            SceneObject sceneObject = new SceneObject();
            sceneObject.objectName = obj.name;
            sceneObject.isEnabled = obj.activeSelf;

            sceneData.objects.Add(sceneObject);
        }
    }
}