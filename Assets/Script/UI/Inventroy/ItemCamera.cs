using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using InventoryUI;

public class ItemCamera : MonoBehaviour
{
    [Header("WeaponObjectInfo"), SerializeField]
    private Info<InventoryItem>[] _weaponObjects;
    [Header("TrinketObjectInfo"), SerializeField]
    private Info<InventoryItem>[] _trinketObjects;

    private Dictionary<InventoryItem, GameObject> _items;
    private const string KEY = "WeaponItem";

    private void Awake()
    {
        UIManager.RegisterUIEvent<(InventoryItem, bool)>(KEY, EnableItem);
        Initialize();
    }

    private void OnDestroy()
    {
        UIManager.UnRegisterUIEvent<(InventoryItem, bool)>(KEY, EnableItem);  
    }

    private void Initialize()
    {
        _items = new Dictionary<InventoryItem, GameObject>();

        AddInfo(_weaponObjects);
        AddInfo(_trinketObjects);
    }

    private void AddInfo(Info<InventoryItem>[] info)
    {
        foreach (var item in info)
            _items.Add(item.Type, item.InfoObject);
    }

    private void EnableItem((InventoryItem itemName, bool enable) tuple)
    {
        if (!_items.TryGetValue(tuple.itemName, out var gameObject))
            return;

        gameObject.SetActive(tuple.enable);
    }
}
