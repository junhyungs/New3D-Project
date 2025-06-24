using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

public class ObjectPool<T> : Singleton_MonoBehaviour<T> where T : MonoBehaviour
{
    protected Dictionary<ObjectKey, Pool> _poolDictionary = new Dictionary<ObjectKey, Pool>();

    protected virtual void Awake()
    {
        DataManager.Instance.TestLoadPathData(); //테스트 코드
    }

    protected virtual PathData GetPathData(ObjectKey objectKey)
    {
        var key = objectKey.ToString();
        var newPathData = DataManager.Instance.GetData(key) as PathData;
        return newPathData;
    }

    protected IEnumerator LoadPrefab(Pool pool, string path, string name, int count)
    {
        var request = Resources.LoadAsync<GameObject>(path);
        yield return new WaitUntil(() => request.isDone);

        var requestAsset = request.asset as GameObject;
        requestAsset.name = name;
        pool.SaveItem = requestAsset;

        for (int i = 0; i < count; i++)
        {
            var poolItem = Instantiate(requestAsset, pool.PoolTrasnform);
            poolItem.SetActive(false);
            pool.Enqueue(poolItem);

            if (pool.SaveItem == null)
                pool.SaveItem = poolItem;
        }
    }
}

public interface IObjectPool
{
    void CreatePool(ObjectKey objectKey, int count = 1);
    void AllDisableGameObject(ObjectKey objectKey);
    void EnqueueGameObject(ObjectKey objectKey, GameObject gameObject);
    GameObject DequeueGameObject(ObjectKey objectKey);
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
