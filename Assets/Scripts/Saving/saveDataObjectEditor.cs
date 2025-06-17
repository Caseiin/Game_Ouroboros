using UnityEngine;
using UnityEditor;
public class saveDataObjectEditor : Editor
{
    SerializedProperty saveDataProp;

    void OnEnable()
    {
        if (target == null) return;
        saveDataProp = serializedObject.FindProperty("saveData");
    }

    public override void OnInspectorGUI()
    {
        if (target == null) return;

        serializedObject.Update();
        EditorGUILayout.PropertyField(saveDataProp, includeChildren: true);
        serializedObject.ApplyModifiedProperties();
    }
}
