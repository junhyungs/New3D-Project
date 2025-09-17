using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponPool : ObjectPool<WeaponPool>
{
    private Dictionary<string, PlayerWeaponObjectContainer> _weaponDictionary = new Dictionary<string, PlayerWeaponObjectContainer>();

    public override void CreatePool(string addressablesKey)
    {
        if (_weaponDictionary.ContainsKey(addressablesKey))
            return;

        var gameObjectName = TrimStart(addressablesKey);
        var poolObject = new GameObject(gameObjectName + "Pool");
        poolObject.transform.SetParent(transform);

        StartCoroutine(LoadPrefab(poolObject.transform, addressablesKey, gameObjectName));
    }

    private IEnumerator LoadPrefab(Transform poolTransform, string addressablesKey, string name)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(addressablesKey);
        yield return handle;

        if (handle.Status != AsyncOperationStatus.Succeeded)
            yield break;

        var prefab = handle.Result;
        prefab.name = name;

        var container = new PlayerWeaponObjectContainer(poolTransform, 5);
        for(int i = 0; i < container.ObjectArray.Length; i++)
        {
            var poolIem = Instantiate(prefab, poolTransform);
            poolIem.SetActive(false);
            container.ObjectArray[i] = poolIem;
        }

        _weaponDictionary.Add(addressablesKey, container);
    }

    public GameObject[] GetWeaponItem(string addressablesKey)
    {
        if(_weaponDictionary.TryGetValue(addressablesKey, out var weaponItems))
        {
            ParentSetting(weaponItems.ObjectArray);
            return weaponItems.ObjectArray;
        }

        return null;
    }

    public void SetWeaponItem(GameObject[] items, string addressablesKey)
    {
        if (!_weaponDictionary.TryGetValue(addressablesKey, out var weaponItems))
            return;

        ParentSetting(weaponItems.ObjectArray, weaponItems.PoolTransform);
    }

    private void ParentSetting(GameObject[] items, Transform parentTransform = null)
    {
        foreach(var item in items)
        {
            item.transform.SetParent(parentTransform);
            if(parentTransform != null)
                item.SetActive(false);
        }
    }
}

