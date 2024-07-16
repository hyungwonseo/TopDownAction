using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GlobalRecord))]
public class GlobalRecordEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GlobalRecord globalRecord = (GlobalRecord)target;

        if (GUILayout.Button("Load From JSON"))
        {
            globalRecord.LoadFromJson();
            EditorUtility.SetDirty(globalRecord);
            AssetDatabase.SaveAssets();
        }
    }
}