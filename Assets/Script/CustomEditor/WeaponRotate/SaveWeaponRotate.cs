using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using UnityEditor;
using System.IO;
using SO;

#if UNITY_EDITOR
public class SaveWeaponRotate : MonoBehaviour
{
    [Header("FileName")]
    public string FileName;
    [Header("WeaponType")]
    public ItemType ItemType;
    [Header("HandType")]
    public PlayerHand HandType;
    [Header("WeaponObject")]
    public GameObject WeaponObject;

    [Header("Path")]
    private const string PATH = "Assets/Resources/Player/ScriptableObject";
    
    public void SaveLocalRotation()
    {
        var scriptableObject = ScriptableObject.CreateInstance<WeaponTransform>();

        scriptableObject.WeaponType = ItemType;
        scriptableObject.HandType = HandType;

        scriptableObject.LocalEulerAngles = WeaponObject.transform.localEulerAngles;
        scriptableObject.LocalPosition = WeaponObject.transform.localPosition;

        string assetPath = AssetDatabase.GenerateUniqueAssetPath(PATH + $"/{FileName}.asset");

        AssetDatabase.CreateAsset(scriptableObject, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public bool IsDirectory()
    {
        return Directory.Exists(PATH);
    }
}
#endif