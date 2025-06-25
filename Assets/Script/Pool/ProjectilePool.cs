using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : ObjectPool<ProjectilePool>, IObjectPool
{
    public void CreatePool(string addressablesKey, int count = 1)
    {
        if (_poolDictionary.ContainsKey(addressablesKey))
            return;

        var gameObjectName = TrimStart(addressablesKey);

        var poolObject = new GameObject(gameObjectName + "Pool");
        poolObject.transform.SetParent(transform);

        var pool = new Pool(poolObject.transform);
        _poolDictionary.Add(addressablesKey, pool);

        StartCoroutine(LoadPrefab(pool, addressablesKey, gameObjectName, count));
    }

    public void AllDisableGameObject(string addressablesKey)
    {
        if (_poolDictionary.TryGetValue(addressablesKey, out var pool))
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

    public void EnqueueGameObject(string addressablesKey, GameObject gameObject)
    {
        if (!_poolDictionary.ContainsKey(addressablesKey))
            CreatePool(addressablesKey);

        var pool = _poolDictionary[addressablesKey];
        gameObject.transform.SetParent(pool.PoolTrasnform);
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }

    public GameObject DequeueGameObject(string addressablesKey)
    {
        if (!_poolDictionary.ContainsKey(addressablesKey))
            CreatePool(addressablesKey);

        var pool = _poolDictionary[addressablesKey];
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
