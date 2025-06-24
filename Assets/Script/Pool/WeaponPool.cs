using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : ObjectPool<WeaponPool>
{
    private Dictionary<ItemType, WeaponObjectPool> _weaponDictionary = new Dictionary<ItemType, WeaponObjectPool>();
    private Dictionary<ItemType, string> _prefabKeys;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    InitializePrefabKey();
    //}

    private void Awake()
    {
        InitializePrefabKey();
    }

    private void InitializePrefabKey()
    {
        _prefabKeys = new Dictionary<ItemType, string>()
        {
            {ItemType.Sword, "PlayerSwordPrefab" },
            {ItemType.Hammer, "PlayerHammerPrefab" },
            {ItemType.Dagger, "PlayerDaggerPrefab" },
            {ItemType.GreatSword, "PlayerGreatSwordPrefab" },
            {ItemType.Umbrella, "PlayerUmbrellaPrefab" }
        };
    }

    private PathData GetPathData(string key)
    {
        var newPathData = DataManager.Instance.GetData(key) as PathData;
        return newPathData;
    }

    public void CreatePool(ItemType weaponKey)
    {
        if (_weaponDictionary.ContainsKey(weaponKey))
            return;

        var gameObjectName = weaponKey.ToString();

        var poolObject = new GameObject(gameObjectName + "Pool");
        poolObject.transform.SetParent(transform);

        var pathKey = _prefabKeys[weaponKey];
        var pathData = GetPathData(pathKey);
        LoadPrefab(poolObject.transform, weaponKey, pathData.Path, gameObjectName);
    }

    private void LoadPrefab(Transform poolTransform, ItemType key, string path, string name)
    {
        var requestAsset = Resources.Load<GameObject>(path);
        requestAsset.name = name;

        var pool = new WeaponObjectPool(poolTransform, 5);
        for (int i = 0; i < pool.ObjectArray.Length; i++)
        {
            var poolItem = Instantiate(requestAsset, poolTransform);
            poolItem.SetActive(false);
            pool.ObjectArray[i] = poolItem;
        }

        _weaponDictionary.Add(key, pool);
    }

    public GameObject[] GetWeaponItem(ItemType objectKey)
    {
        if(_weaponDictionary.TryGetValue(objectKey, out var weaponItems))
        {
            ParentSetting(weaponItems.ObjectArray);
            return weaponItems.ObjectArray;
        }

        CreatePool(objectKey);

        var weaponItem = GetWeaponItem(objectKey);
        return weaponItem;
    }

    public void SetWeaponItem(GameObject[] items, ItemType objectKey)
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
            if(parentTransform != null)
                item.SetActive(false);
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
