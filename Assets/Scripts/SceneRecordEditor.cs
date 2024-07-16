using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneRecord))]
public class SceneRecordEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneRecord sceneRecord = (SceneRecord)target;

        if (GUILayout.Button("Load From JSON"))
        {
            sceneRecord.LoadFromJson();
            EditorUtility.SetDirty(sceneRecord);
            AssetDatabase.SaveAssets();
        }
    }
}