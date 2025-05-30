using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

public class ObjectPool : Singleton_MonoBehaviour<ObjectPool>
{
    private Dictionary<ObjectName, Pool> _poolDictionary = new Dictionary<ObjectName, Pool>();
    private Dictionary<ObjectName, PathData> _pathDataDictionary = new Dictionary<ObjectName, PathData>();

    private PathData GetPathData(ObjectName objectName)
    {
        if (_pathDataDictionary.TryGetValue(objectName, out var pathData))
            return pathData;

        return new PathData("Test", "Test"); // 없으면 데이터 매니저에서 끌어오기.
    }

    public void CreatePool(ObjectName name, int count = 1)
    {
        if (_poolDictionary.ContainsKey(name))
            return;

        var gameObjectName = name.ToString();

        var poolObject = new GameObject(gameObjectName + "Pool");
        poolObject.transform.SetParent(transform);

        var pool = new Pool(poolObject.transform);
        _poolDictionary.Add(name, pool);

        var pathData = GetPathData(name);
        StartCoroutine(LoadPrefab(pool, pathData.Path, gameObjectName, count));
    }

    private IEnumerator LoadPrefab(Pool pool, string path, string name, int count)
    {
        var request = Resources.LoadAsync<GameObject>(path);
        yield return new WaitUntil(() => request.isDone);

        var gameObject = Instantiate(request.asset as GameObject);
        gameObject.name = name;
        pool.SaveItem = gameObject;

        for(int i = 0; i < count; i++)
        {
            var poolItem = Instantiate(gameObject);
            poolItem.SetActive(false);
            pool.Enqueue(poolItem);
        }
    }

    public void AllDisableGameObject(ObjectName objectName)
    {
        if(_poolDictionary.TryGetValue(objectName, out var pool))
        {
            foreach(Transform childTransform in pool.PoolTrasnform)
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

    public void EnqueueGameObject(ObjectName objectName, GameObject gameObject)
    {
        if(!_poolDictionary.ContainsKey(objectName))
            CreatePool(objectName);

        var pool = _poolDictionary[objectName];
        gameObject.transform.SetParent(pool.PoolTrasnform);
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }

    public GameObject DequeueGameObject(ObjectName objectName)
    {
        if(!_poolDictionary.ContainsKey(objectName))
            CreatePool(objectName);

        var pool = _poolDictionary[objectName];
        if(pool.QueueCount == 0)
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
