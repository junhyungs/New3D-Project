using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyPool : ObjectPool<EnemyPool>
{
    private Dictionary<string, ObjectContainer> _poolDictionary = new Dictionary<string, ObjectContainer>();

    public override void CreatePool(string addressablesKey, int count = 1)
    {
        if (!_poolDictionary.ContainsKey(addressablesKey))
            return;

        var gameObjectName = TrimStart(addressablesKey);

        var poolObject = new GameObject(gameObjectName + "Pool");
        poolObject.transform.SetParent(transform);

        var container = new ObjectContainer(poolObject.transform);
        _poolDictionary.Add(addressablesKey, container);
        StartCoroutine(LoadPrefab(container, addressablesKey, gameObjectName, count));
    }

    private IEnumerator LoadPrefab(ObjectContainer container, string path, string name, int count)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(path);
        yield return handle;

        if(handle.Status != AsyncOperationStatus.Succeeded)
            yield break;

        var prefab = handle.Result;
        prefab.name = name;
        container.SaveItem = prefab;

        for(int i = 0; i < count; i++)
        {
            var item = Instantiate(prefab, container.ContainerTransform);
            item.SetActive(false);
            container.Enqueue(item);
        }
    }

    public GameObject DequeueGameObject(string addressablesKey)
    {
        if(!_poolDictionary.ContainsKey(addressablesKey))
            CreatePool(addressablesKey);

        var container = _poolDictionary[addressablesKey];
        if(container.QueueCount == 0)
        {
            var saveItem = container.SaveItem;
            var newObject = Instantiate(saveItem);
            return newObject;
        }

        var gameObject = container.Dequeue();
        gameObject.transform.parent = null;
        return gameObject;
    }

    public void EnqueueGameObject(string addressablesKey, GameObject gameObject)
    {
        if (!_poolDictionary.ContainsKey(addressablesKey))
            CreatePool(addressablesKey);

        var container = _poolDictionary[addressablesKey];
        gameObject.transform.SetParent(container.ContainerTransform);
        gameObject.SetActive(false);
        container.Enqueue(gameObject);
    }

    public void AllDisableGameObject(string addressablesKey)
    {
        if (_poolDictionary.TryGetValue(addressablesKey, out var container))
        {
            foreach (Transform childTransform in container.ContainerTransform)
            {
                var gameObject = childTransform.gameObject;

                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                    container.Enqueue(gameObject);
                }
            }
        }
    }
}
