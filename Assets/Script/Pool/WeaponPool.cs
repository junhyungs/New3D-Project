using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : ObjectPool<WeaponPool>
{
    private Dictionary<ObjectKey, WeaponObjectPool> _weaponDictionary = new Dictionary<ObjectKey, WeaponObjectPool>();

    public void CreatePool(ObjectKey objectKey)
    {
        if (_weaponDictionary.ContainsKey(objectKey))
            return;

        var gameObjectName = objectKey.ToString();

        var poolObject = new GameObject(gameObjectName + "Pool");
        poolObject.transform.SetParent(transform);

        var pathData = GetPathData(objectKey);
        StartCoroutine(LoadPrefab(poolObject.transform, objectKey, pathData.Path, gameObjectName));
    }

    private IEnumerator LoadPrefab(Transform poolTransform, ObjectKey key, string path, string name)
    {
        var request = Resources.LoadAsync<GameObject>(path);
        yield return new WaitUntil(() => request.isDone);

        var requestAsset = request.asset as GameObject;
        requestAsset.name = name;

        var pool = new WeaponObjectPool(poolTransform, 3);
        for(int i = 0; i < pool.ObjectArray.Length; i++)
        {
            var poolItem = Instantiate(requestAsset, poolTransform);
            poolItem.SetActive(false);
            pool.ObjectArray[i] = poolItem;
        }

        _weaponDictionary.Add(key, pool);
    }

    public GameObject[] GetWeaponItem(ObjectKey objectKey)
    {
        if(_weaponDictionary.TryGetValue(objectKey, out var weaponItems))
        {
            ParentSetting(weaponItems.ObjectArray);
            return weaponItems.ObjectArray;
        }

        return null;
    }

    public void SetWeaponItem(GameObject[] items, ObjectKey objectKey)
    {
        if (!_weaponDictionary.TryGetValue(objectKey, out var weaponItems))
            return;

        ParentSetting(weaponItems.ObjectArray, weaponItems.PoolTransform);
    }

    private void ParentSetting(GameObject[] items, Transform parentTransform = null)
    {
        foreach(var item in items)
        {
            item.transform.SetParent(parentTransform);
        }
    }
}

public class WeaponObjectPool
{
    public WeaponObjectPool(Transform poolTransform, int arrayLength)
    {
        ObjectArray = new GameObject[arrayLength];
        PoolTransform = poolTransform;
    }

    public GameObject[] ObjectArray { get; private set; }
    public Transform PoolTransform { get; private set; }
}
