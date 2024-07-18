using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Threading.Tasks;

#if !USE_SCRIPTABLE_OBJECT
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

    float checkInterval = 0.2f;
    float tempTime = 0;

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
            // global과 scene으로 나눠져있던 로드 함수를 통합
            globalData = LoadData<GlobalData>(filePathGlobal);

            PlayerController.hp = globalData.hp;
            ItemKeeper.hasArrows = globalData.arrows;
            ItemKeeper.hasKeys = globalData.keys;
        }
        else 
        {
            globalData.hp = PlayerController.hp;
            globalData.arrows = ItemKeeper.hasArrows;
            globalData.keys = ItemKeeper.hasKeys;
            // global과 scene으로 나눠져있던 세이브 함수를 통합
            SaveData<GlobalData>(globalData, filePathGlobal);
        }

        filePathScene = Path.Combine(Application.persistentDataPath
            , SceneManager.GetActiveScene().name + ".json");
        if (File.Exists(filePathScene))
        {
            // global과 scene으로 나눠져있던 로드 함수를 통합
            sceneData = LoadData<SceneData>(filePathScene);

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
            // global과 scene으로 나눠져있던 세이브 함수를 통합
            SaveData<SceneData>(sceneData, filePathScene);
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
                // global과 scene으로 나눠져있던 세이브 함수를 통합
                SaveData<GlobalData>(globalData, filePathGlobal);
            }
            tempTime = 0;
        }
        
    }    

    public T LoadData<T>(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        T data = JsonConvert.DeserializeObject<T>(jsonData);
        Debug.Log("Data loaded from " + filePath);
        return data;
    }

    // 동기방식 세이브
    /*public void SaveData<T>(T data, string filePath)
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Data saved to " + filePath);
    }*/
    
    // 비동기방식 세이브
    public async void SaveData<T>(T data, string filePath)
    {
        await SaveDataAsync<T>(data, filePath); // 저장 기능은 비동기 처리함
    }

    async Task SaveDataAsync<T>(T data, string filePath) // Task를 리턴하는 비동기 함수
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            await writer.WriteAsync(jsonData);
        }
        Debug.Log("Data saved to " + filePath);
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

    public void SetSceneData(string name, bool value)
    {
        bool dataChanged = false;
        foreach (SceneObject obj in sceneData.objects)
        {
            if (obj.objectName == name)
            {
                obj.isEnabled = value;
                dataChanged = true;
            }            
        }
        if (dataChanged)
        {
            // global과 scene으로 나눠져있던 세이브 함수를 통합
            SaveData<SceneData>(sceneData, filePathScene);
        }        
    }
}
#endif