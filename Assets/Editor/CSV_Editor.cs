using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

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
        EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(_converter.FileName));

        if (GUILayout.Button("Parse"))
        {
            if (!IsParse())
                return;

            _converter.ParseCSVData();
        }

        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Clear"))
        {
            _converter.ClearFileName();
        }
    }

    private bool IsParse()
    {
        var path = _converter.GetCSVPath();
        Debug.Log(path);

        return File.Exists(path);
    }
}
