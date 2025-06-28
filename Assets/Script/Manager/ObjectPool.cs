using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectPool<T> : Singleton_MonoBehaviour<T> where T : MonoBehaviour
{
    protected Dictionary<string, Pool> _poolDictionary = new Dictionary<string, Pool>();

    protected string TrimStart(string targetString)
    {
        var trim = "Prefab/";
        if(targetString.StartsWith(trim))
            return targetString.Substring(trim.Length);

        return targetString;
    }

    protected IEnumerator LoadPrefab(Pool pool, string path, string name, int count)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(path);
        yield return handle;

        if(handle.Status != AsyncOperationStatus.Succeeded)
            yield break;

        var prefab = handle.Result;
        prefab.name = name;
        pool.SaveItem = prefab;

        for (int i = 0; i < count; i++)
        {
            var poolItem = Instantiate(prefab, pool.PoolTrasnform);
            poolItem.SetActive(false);
            pool.Enqueue(poolItem);

            if (pool.SaveItem == null)
                pool.SaveItem = poolItem;
        }
    }
}

public interface IObjectPool
{
    void CreatePool(string address, int count = 1);
    void AllDisableGameObject(string address);
    void EnqueueGameObject(string addressablesKey, GameObject gameObject);
    GameObject DequeueGameObject(string addressablesKey);
}

public class Pool
{
    public Pool(Transform poolTransform)
    {
        _objectQueue = new Queue<GameObject>();
        PoolTrasnform = poolTransform;
    }

    private Queue<GameObject> _objectQueue;
    public Transform PoolTrasnform { get; private set; }
    public GameObject SaveItem { get; set; }
    public int QueueCount => _objectQueue.Count;
    
    public GameObject Dequeue()
    {
        var gameObject = _objectQueue.Dequeue();
        return gameObject;
    }

    public void Enqueue(GameObject gameObject)
    {
        _objectQueue.Enqueue(gameObject);
    }
}
