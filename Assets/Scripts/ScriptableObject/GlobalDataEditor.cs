using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GlobalData))]
public class GlobalDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GlobalData globalData = (GlobalData)target;

        if (GUILayout.Button("Load From JSON"))
        {
            globalData.LoadFromJson();
            EditorUtility.SetDirty(globalData);
            AssetDatabase.SaveAssets();
        }
    }
}