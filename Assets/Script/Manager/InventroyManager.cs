using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using InventoryUI;
using System;
using Item;
using GameData;

public class InventroyManager : Singleton_MonoBehaviour<InventroyManager>
{
    [Header("StartItem"), SerializeField]
    private WeaponItem _sword;

    private Dictionary<ItemType, Slot> _slotDictionary = new Dictionary<ItemType, Slot>();
    private Dictionary<ItemType, Action<IPlayerItem, Slot>> _refreshDictionary = new Dictionary<ItemType, Action<IPlayerItem, Slot>>();
    private Dictionary<ItemType, IPlayerItem> _refreshItemDictionary = new Dictionary<ItemType, IPlayerItem>();

    private void Awake()
    {
        DataManager.Instance.TestLoadItemDescriptionData(); //테스트 코드
    }

    private void Start()
    {
        StartPlayerItem();
    }

    private void StartPlayerItem()
    {
        SetItem(_sword);

        WeaponPool.Instance.CreatePool(_sword.SlotName, _sword.PrefabKey);
        WeaponManager.Instance.SetWeapon(ItemType.Sword);
    }

    public void RegisterSlot(ItemType slotName, Slot slot)
    {
        if (!_slotDictionary.ContainsKey(slotName))
            _slotDictionary.Add(slotName, slot);

        if(_refreshItemDictionary.TryGetValue(slotName, out IPlayerItem item))
        {
            if(_refreshDictionary.TryGetValue(slotName, out Action<IPlayerItem, Slot> callBack))
            {
                callBack?.Invoke(item, slot);
            }
        }
    }

    private Slot GetSlot(ItemType key)
    {
        if (_slotDictionary.TryGetValue(key, out Slot slot))
            return slot;

        return null;
    }

    private void SaveItem(ItemType slotName, IPlayerItem playerItem)
    {
        if (!_refreshItemDictionary.ContainsKey(slotName))
        {
            _refreshItemDictionary.Add(slotName, playerItem);

            _refreshDictionary[slotName] = (item, slot) =>
            {
                InitializeItemSlot(item, slot);
                _refreshDictionary.Remove(slotName);
            };
        }
    }

    public void SetItem(IPlayerItem playerItem)
    {
        var slotName = playerItem.SlotName;
        var slot = GetSlot(slotName);

        if (slot == null)
        {
            SaveItem(slotName, playerItem);
            return;
        }
            
        InitializeItemSlot(playerItem, slot);
    }

    private void InitializeItemSlot(IPlayerItem playerItem, Slot slot)
    {
        var dataKey = playerItem.DescriptionKey;
        if (DataManager.Instance.GetData(dataKey) is not ItemDescriptionData descriptionData)
            return;

        slot.InitializeSlot();
        slot.DescriptionData = descriptionData;

        var canEquip = playerItem.CanEquip;
        if (canEquip)
        {
            SetUpEquipSlot(playerItem, slot);
        }
    }

    private void SetUpEquipSlot(IPlayerItem playerItem, Slot slot)
    {
        switch (slot)
        {
            case WeaponSlot weaponSlot:
                SetUpWeaponSlot(playerItem, weaponSlot);
                break;
        }
    }

    private void SetUpWeaponSlot(IPlayerItem playerItem, WeaponSlot slot)
    {
        if (playerItem is not IPlayerWeaponItem weaponItem)
            return;

        var dataKey = weaponItem.WeaponDataKey;

        WeaponPool.Instance.CreatePool(_sword.SlotName, _sword.PrefabKey);
        if (DataManager.Instance.GetData(dataKey) is not PlayerWeaponData weaponData)
            return;

        slot.WeaponData = weaponData;
    }
}
