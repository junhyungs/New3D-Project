using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using InventoryUI;

public class ItemCamera : MonoBehaviour
{
    [Header("WeaponObjectInfo"), SerializeField]
    private Info<ItemType>[] _weaponObjects;
    [Header("TrinketObjectInfo"), SerializeField]
    private Info<ItemType>[] _trinketObjects;

    private Dictionary<ItemType, GameObject> _items;
    private const string KEY = "WeaponItem";

    private void Awake()
    {
        UIManager.RegisterUIEvent<(ItemType, bool)>(KEY, EnableItem);
        Initialize();
    }

    private void OnDestroy()
    {
        UIManager.UnRegisterUIEvent<(ItemType, bool)>(KEY, EnableItem);  
    }

    private void Initialize()
    {
        _items = new Dictionary<ItemType, GameObject>();

        AddInfo(_weaponObjects);
        AddInfo(_trinketObjects);
    }

    private void AddInfo(Info<ItemType>[] info)
    {
        foreach (var item in info)
            _items.Add(item.Type, item.InfoObject);
    }

    private void EnableItem((ItemType itemName, bool enable) tuple)
    {
        if (!_items.TryGetValue(tuple.itemName, out var gameObject))
            return;

        gameObject.SetActive(tuple.enable);
    }
}
