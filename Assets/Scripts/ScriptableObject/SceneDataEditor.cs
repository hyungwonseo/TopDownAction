using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneData))]
public class SceneRecordEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneData sceneData = (SceneData)target;

        if (GUILayout.Button("Load From JSON"))
        {
            sceneData.LoadFromJson();
            EditorUtility.SetDirty(sceneData);
            AssetDatabase.SaveAssets();
        }
    }
}