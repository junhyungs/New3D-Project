using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CSV_Converter))]
public class CSV_Editor : Editor
{
    private CSV_Converter _converter;

    private void OnEnable()
    {
        _converter = (CSV_Converter)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
    }
}
