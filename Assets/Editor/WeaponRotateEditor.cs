using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveWeaponRotate))]
public class WeaponRotateEditor : Editor
{
    private SaveWeaponRotate _weaponRotate;

    private void OnEnable()
    {
        _weaponRotate = (SaveWeaponRotate)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        //EditorGUI.BeginDisabledGroup(_weaponRotate.IsDirectory());

        if (GUILayout.Button("Create"))
        {
            _weaponRotate.SaveLocalRotation();
        }

        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Clear"))
        {
            _weaponRotate.FileName = string.Empty;
        }
    }
}
