using EnumCollection;
using GameData;
using InventoryUI;
using ItemComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : Singleton_MonoBehaviour<InventoryManager>
{
    [Header("StartingWeapon")]
    [SerializeField] private Item _startingWeapon;
    [Header("StartingItems")]
    [SerializeField] private Item[] _startingItems;

    private PlayerInventoryData _saveData;
    private Dictionary<ItemType, PlayerItemSlot> _slotDictionary = new Dictionary<ItemType, PlayerItemSlot>();
    private Dictionary<ItemType, Action<IInventoryItem, PlayerItemSlot>> _refreshDictionary = new Dictionary<ItemType, Action<IInventoryItem, PlayerItemSlot>>();
    private Dictionary<ItemType, IInventoryItem> _refreshItemDictionary = new Dictionary<ItemType, IInventoryItem>();

    public void InitializeInventory() //TODO 나중에 게임 매니저에서 일괄적으로 관리.
    {
        var key = DataKey.Inventory_Data.ToString();
        var saveData = DataManager.Instance.GetData(key) as PlayerInventoryData;
        if (saveData.InitInventory)
        {
            var saveWeapon = _saveData.SaveWeapon;
            var saveWeaponType = saveWeapon.WeaponType;
            WeaponManager.Instance.SetWeapon(saveWeaponType, saveWeapon);
        }
        else
        {
            if (_startingWeapon is WeaponItem weaponItem)
            {
                weaponItem.StartWeapon();
                SetItem(_startingWeapon);
            }

            foreach (var item in _startingItems)
                SetItem(item);

            saveData.InitInventory = true;
        }

        _saveData = saveData;
    }

    public void RegisterSlot(ItemType slotName, PlayerItemSlot slot)
    {
        if (!_slotDictionary.ContainsKey(slotName))
            _slotDictionary.Add(slotName, slot);

        ApplySaveData(slotName, slot);
        TryInvokeRefreshCallBack(slotName);
    }

    private void TryApply<TSlot, TData>(TSlot slot, TData data, Action<TData> applyAction)
        where TSlot : PlayerItemSlot
        where TData : class
    {
        if(data != null)
        {
            slot.InitializeSlot();
            applyAction?.Invoke(data);
        }
    }

    private void TryApply<TSlot>(TSlot slot, int data, Func<int, bool> checkFuc)
        where TSlot : CurrencyItemSlot
    {
        if(checkFuc != null && checkFuc(data))
        {
            slot.InitializeSlot();
            slot.Currency = data;
        }
    }

    private void ApplySaveData(ItemType slotName, PlayerItemSlot slot)
    {
        if (_saveData == null)
            return;

        switch (slot)
        {
            case WeaponSlot weaponSlot:
                TryApply(weaponSlot, _saveData.GetWeaponData(slotName), 
                    (data) => weaponSlot.WeaponData = data);
                break;
            case InventorySlot inventorySlot:
                TryApply(inventorySlot, _saveData.GetInventoryItemData(slotName),
                    (data) => inventorySlot.InventoryItemData = data);
                break;
            case TrinketSlot trinketSlot:
                TryApply(trinketSlot, _saveData.GetTrinketItemData(slotName), 
                    (data) => trinketSlot.TrinketItemData = data);
                break;
            case SoulSlot soulSlot:
                TryApply(soulSlot, _saveData.SoulCount, (data) => data > 0);
                break;
            case SeedSlot seedSlot:
                TryApply(seedSlot, _saveData.SoulCount, (data) => data > 0);
                break;
        }
    }

    private void TryInvokeRefreshCallBack(ItemType slotName)
    {
        if (_refreshItemDictionary.TryGetValue(slotName, out IInventoryItem item))
        {
            if (_refreshDictionary.TryGetValue(slotName, out Action<IInventoryItem, PlayerItemSlot> callBack))
            {
                var playerItemslot = GetSlot(slotName);
                callBack?.Invoke(item, playerItemslot);
            }
        }
    }

    private PlayerItemSlot GetSlot(ItemType key)
    {
        if (_slotDictionary.TryGetValue(key, out PlayerItemSlot slot))
            return slot;

        return null;
    }

    public void SetItem(IInventoryItem item)
    {
        SetInventoryItem(item);
    }

    public bool CanUseCurrencyItem(ItemType slotName, int count)
    {
        var slot = GetSlot(slotName) as CurrencyItemSlot;
        if (!slot.CanUseCurrencyItem(count))
            return false;
        
        slot.UseItem(count);
        return true;
    }

    #region PlayerItem
    private void SetInventoryItem(IInventoryItem inventoryItem)
    {
        var slotName = inventoryItem.SlotName;
        var slot = GetSlot(slotName);
        if(slot != null)
            InitializeInventorySlot(inventoryItem, slot, slotName);
        else
            RefreshItem(slotName, inventoryItem);
    }

    private void InitializeInventorySlot(IInventoryItem inventoryItem, PlayerItemSlot slot,
        ItemType slotName)
    {
        slot.InitializeSlot();
        var itemDataSO = inventoryItem.ItemDataSO;
        if (itemDataSO == null)
            return;

        bool canEquip = itemDataSO.Equip;
        if (canEquip && slot is WeaponSlot weaponSlot)
        {
            var playerWeaponDataSO = itemDataSO as PlayerWeaponDataSO;
            if(playerWeaponDataSO != null)
            {
                var weaponData = playerWeaponDataSO.WeaponData;
                weaponSlot.WeaponData = weaponData;
                WeaponPool.Instance.CreatePool(playerWeaponDataSO.AddressKey);

                var targetDictionary = _saveData.WeaponDataDictionary;
                SaveItem(targetDictionary, slotName, weaponData);
            }
        }
        else
        {
            switch (slot)
            {
                case InventorySlot inventorySlot:
                    var inventoryItemDataSO = itemDataSO as InventoryItemDataSO;
                    if(inventoryItemDataSO != null)
                    {
                        var inventoryData = inventoryItemDataSO.InventoryItemData;
                        inventorySlot.InventoryItemData = inventoryData;
                        inventorySlot.OnItemUI();

                        var targetDictionary = _saveData.InventoryDataDictionary;
                        SaveItem(targetDictionary, slotName, inventoryData);
                    }
                    break;
                case TrinketSlot trinketSlot:
                    var trinketItemDataSO = itemDataSO as TrinketItemDataSO;
                    if(trinketItemDataSO != null)
                    {
                        var trinketData = trinketItemDataSO.TrinketItemData;
                        trinketSlot.TrinketItemData = trinketData;

                        var targetDictionary = _saveData.TrinketDataDictionary;
                        SaveItem(targetDictionary, slotName, trinketData);
                    }
                    break;
                case SoulSlot soulSlot:
                    var soulItemDataSO = itemDataSO as SoulItemDataSO;
                    if(soulItemDataSO != null &&
                        inventoryItem is ICurrencyItem soulItem)
                    {
                        soulSlot.Currency += soulItem.GetValue();
                        soulSlot.SaveCurrency(_saveData);
                    }
                    break;
                case SeedSlot seedSlot:
                    var seedItemDataSO = itemDataSO as SeedItemDataSO;
                    if(seedItemDataSO != null &&
                        inventoryItem is ICurrencyItem seedItem)
                    {
                        seedSlot.Currency += seedItem.GetValue();
                        seedSlot.SaveCurrency(_saveData);
                    }
                    break;
            }
        }
    }

    private void SaveItem<T>(Dictionary<ItemType, T> targetDictionary, ItemType itemType, T tData)
    {
        if (targetDictionary == null)
            return;

        if (!targetDictionary.ContainsKey(itemType))
            targetDictionary.Add(itemType, tData);
    }

    private void RefreshItem(ItemType slotName, IInventoryItem inventoryItem)
    {
        if (!_refreshItemDictionary.ContainsKey(slotName))
        {
            _refreshItemDictionary.Add(slotName, inventoryItem);

            _refreshDictionary[slotName] = (item, slot) =>
            {
                InitializeInventorySlot(item, slot, slotName);
                _refreshDictionary.Remove(slotName);
            };
        }
    }
    #endregion
}
