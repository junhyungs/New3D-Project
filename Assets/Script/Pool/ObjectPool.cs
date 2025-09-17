using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using EnemyComponent;

public class ObjectPool<T> : Singleton_MonoBehaviour<T> where T : MonoBehaviour
{
    public virtual void CreatePool(string addressablesKey) { }
    public virtual void CreatePool(string addressablesKey, int count = 1) { }    
    protected string TrimStart(string targetString)
    {
        var trim = "Prefab/";
        if(targetString.StartsWith(trim))
            return targetString.Substring(trim.Length);

        return targetString;
    }
}

