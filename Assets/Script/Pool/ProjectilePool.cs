using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : ObjectPool<ProjectilePool>, IObjectPool
{
    public void CreatePool(ObjectKey objectKey, int count = 1)
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

    public void AllDisableGameObject(ObjectKey objectKey)
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

    public void EnqueueGameObject(ObjectKey objectKey, GameObject gameObject)
    {
        if (!_poolDictionary.ContainsKey(objectKey))
            CreatePool(objectKey);

        var pool = _poolDictionary[objectKey];
        gameObject.transform.SetParent(pool.PoolTrasnform);
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }

    public GameObject DequeueGameObject(ObjectKey objectKey)
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
