using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

#if UNITY_EDITOR
public class ImageMaker : MonoBehaviour
{
    [Header("TargetCamera"), SerializeField]
    public Camera _targetCamera;
    [Header("RenderTexture Sample"), SerializeField]
    public RenderTexture _renderTexture;
    [Header("FileName"), SerializeField]
    public string _imageName;

    public void CaptureImage()
    {
        if(_targetCamera == null)
        {
            Debug.Log("camera null");
            return;
        }

        var width = _renderTexture.width;
        var height = _renderTexture.height;

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        _targetCamera.targetTexture = renderTexture;
        _targetCamera.Render();

        RenderTexture.active = renderTexture;
        Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();

        byte[] bytes = texture2D.EncodeToPNG();

        string path = Path.Combine(Application.dataPath, "UI/CaptureImage", _imageName + "CaptureImage.png");
        File.WriteAllBytes(path, bytes);

        _targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Object.DestroyImmediate(renderTexture);
        Object.DestroyImmediate(texture2D);

        AssetDatabase.Refresh();
    }
}
#endif