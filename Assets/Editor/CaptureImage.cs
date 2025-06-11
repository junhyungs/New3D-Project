using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImageMaker))]
public class CaptureImage : Editor
{
    private ImageMaker _maker;

    private void OnEnable()
    {
        _maker = (ImageMaker)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(_maker._renderTexture == null ||
            _maker._imageName == string.Empty);

        if (GUILayout.Button("Save"))
        {
            _maker.CaptureImage();
        }

        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Clear"))
        {
            _maker._imageName = string.Empty;
        }
    }
}
