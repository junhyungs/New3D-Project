using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCombiner))]
public class MeshCombine_Editor : Editor
{
    private MeshCombiner _combiner;
    private void OnEnable()
    {
        _combiner = (MeshCombiner)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(_combiner.GameObjects == null);

        if (GUILayout.Button("Combine"))
        {
            _combiner.MeshCombine();
        }

        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Clear"))
        {
            _combiner.ResetMeshCombiner();
        }
    }
}
