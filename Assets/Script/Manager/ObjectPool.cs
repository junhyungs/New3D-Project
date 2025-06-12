using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

public class ObjectPool<T> : Singleton_MonoBehaviour<T> where T : MonoBehaviour
{
    protected Dictionary<ObjectKey, Pool> _poolDictionary = new Dictionary<ObjectKey, Pool>();

    private void Awake()
    {
        DataManager.Instance.TestLoadPathData(); //테스트 코드
    }

    protected PathData GetPathData(ObjectKey objectKey)
    {
        var key = objectKey.ToString();
        var newPathData = DataManager.Instance.GetData(key) as PathData;
        return newPathData;
    }

    public virtual void CreatePool(ObjectKey objectKey, int count = 1)
    {
        if (_poolDictionary.ContainsKey(objectKey))
            return;

        var gameObjectName = objectKey.ToString();

        var poolObject = new GameObject(gameObjectName + "Pool");
        poolObject.transform.SetParent(transform);

        var pool = new Pool(poolObject.transform);
        _poolDictionary.Add(objectKey, pool);

        var pathData = GetPathData(objectKey);
        StartCoroutine(LoadPrefab(pool, pathData.Path, gameObjectName, count));
    }

    private IEnumerator LoadPrefab(Pool pool, string path, string name, int count)
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

    public virtual void AllDisableGameObject(ObjectKey objectKey)
    {
        if (_poolDictionary.TryGetValue(objectKey, out var pool))
        {
            foreach (Transform childTransform in pool.PoolTrasnform)
            {
                var gameObject = childTransform.gameObject;

                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                    pool.Enqueue(gameObject);
                }
            }
        }
    }

    public virtual void EnqueueGameObject(ObjectKey objectKey, GameObject gameObject)
    {
        if (!_poolDictionary.ContainsKey(objectKey))
            CreatePool(objectKey);

        var pool = _poolDictionary[objectKey];
        gameObject.transform.SetParent(pool.PoolTrasnform);
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }

    public virtual GameObject DequeueGameObject(ObjectKey objectKey)
    {
        if (!_poolDictionary.ContainsKey(objectKey))
            CreatePool(objectKey);

        var pool = _poolDictionary[objectKey];
        if (pool.QueueCount == 0)
        {
            var saveItem = pool.SaveItem;
            var newgameObject = Instantiate(saveItem);

            return newgameObject;
        }

        var gameObject = pool.Dequeue();
        gameObject.transform.parent = null;
        gameObject.SetActive(true);

        return gameObject;
    }
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
