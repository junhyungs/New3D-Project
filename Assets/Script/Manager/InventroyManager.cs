using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using InventoryUI;
using System;
using ItemComponent;
using GameData;

public class InventoryManager : Singleton_MonoBehaviour<InventoryManager>
{
    [Header("StartItem"), SerializeField]
    private List<Item> _startItems;
    private Dictionary<ItemType, ISlot> _slotDictionary = new Dictionary<ItemType, ISlot>();
    private Dictionary<ItemType, Action<IPlayerItem, PlayerItemSlot>> _refreshDictionary = new Dictionary<ItemType, Action<IPlayerItem, PlayerItemSlot>>();
    private Dictionary<ItemType, IPlayerItem> _refreshItemDictionary = new Dictionary<ItemType, IPlayerItem>();
    private PlayerInventoryData _playerInventoryData;
    
    //private void Awake()
    //{
    //    DataManager.Instance.TestLoadItemDescriptionData(); //테스트 코드
    //}

    private void Start()
    {
        var key = DataKey.Inventory_Data.ToString();
        _playerInventoryData = DataManager.Instance.GetData(key) as PlayerInventoryData;

        CreateCurrencySlot();
        StartPlayerItem();
    }

    private void CreateCurrencySlot()
    {
        SeedSlot seedSlot = new SeedSlot();
        SoulSlot soulSlot = new SoulSlot();
        RegisterSlot(ItemType.Seed, seedSlot);
        RegisterSlot(ItemType.Soul, soulSlot);
    }

    private void StartPlayerItem()
    {
        foreach (var item in _startItems)
            SetPlayerItem(item);
    }

    public void RegisterSlot(ItemType slotName, ISlot slot)
    {
        if (!_slotDictionary.ContainsKey(slotName))
            _slotDictionary.Add(slotName, slot);

        ApplySavedDescriptionData(slotName, slot);
        ApplySavedWeaponData(slotName, slot);

        TryInvokeRefreshCallBack(slotName);
    }

    private void ApplySavedDescriptionData(ItemType slotName, ISlot slot)
    {
        var saveDescriptionDic = _playerInventoryData.DescriptionDataDictionary;
        if (saveDescriptionDic.TryGetValue(slotName, out var itemDescriptionData))
        {
            var itemSlot = slot as PlayerItemSlot;
            itemSlot.InitializeSlot();
            itemSlot.DescriptionData = itemDescriptionData;
        }
    }

    private void ApplySavedWeaponData(ItemType slotName, ISlot slot)
    {
        var equipItemSet = _playerInventoryData.EquipItemSet;
        if (equipItemSet.Contains(slotName))
        {
            var saveWeaponDic = _playerInventoryData.WeaponDataDictionary;
            if (saveWeaponDic.TryGetValue(slotName, out var itemWeaponData))
            {
                var weaponSlot = slot as WeaponSlot;
                weaponSlot.WeaponData = itemWeaponData;
            }
        }
    }

    private void TryInvokeRefreshCallBack(ItemType slotName)
    {
        if (_refreshItemDictionary.TryGetValue(slotName, out IPlayerItem item))
        {
            if (_refreshDictionary.TryGetValue(slotName, out Action<IPlayerItem, PlayerItemSlot> callBack))
            {
                var playerItemslot = GetSlot(slotName) as PlayerItemSlot;
                callBack?.Invoke(item, playerItemslot);
            }
        }
    }

    private ISlot GetSlot(ItemType key)
    {
        if (_slotDictionary.TryGetValue(key, out ISlot slot))
            return slot;

        return null;
    }

    public void SetGameItem(IGameItem gameItem)
    {
        switch (gameItem)
        {
            case IPlayerItem playerItem:
                SetPlayerItem(playerItem);
                break;
            case ICurrencyItem currencyItem:
                SetCurrencyItem(currencyItem);
                break;
        }
    }

    private void SetCurrencyItem(ICurrencyItem currencyItem)
    {
        var slotName = currencyItem.SlotName;
        var slot = GetSlot(slotName) as CurrencyItemSlot;
        slot.Currency += currencyItem.GetValue();
        SaveCurrency(slot);
    }

    public bool CanUseCurrencyItem(ItemType slotName, int count)
    {
        var slot = GetSlot(slotName) as CurrencyItemSlot;
        if (!slot.CanUseCurrencyItem(count))
            return false;
        
        slot.UseItem(count);
        SaveCurrency(slot);
        return true;
    }

    private void SaveCurrency(CurrencyItemSlot slot)
    {
        switch (slot.GetSlotType)
        {
            case CurrencyItemSlot.CurrencySlot.Seed:
                _playerInventoryData.SeedCount = slot.Currency;
                break;
            case CurrencyItemSlot.CurrencySlot.Soul:
                _playerInventoryData.SoulCount = slot.Currency;
                break;
        }
    }

    #region PlayerItem
    private void SetPlayerItem(IPlayerItem playerItem)
    {
        var slotName = playerItem.SlotName;
        var slot = GetSlot(slotName) as PlayerItemSlot;
        if(slot != null)
            InitializePlayerItemSlot(playerItem, slot);
        else
            SaveItem(slotName, playerItem);
    }

    private void SaveItem(ItemType slotName, IPlayerItem playerItem)
    {
        if (!_refreshItemDictionary.ContainsKey(slotName))
        {
            _refreshItemDictionary.Add(slotName, playerItem);

            _refreshDictionary[slotName] = (item, slot) =>
            {
                InitializePlayerItemSlot(item, slot);
                _refreshDictionary.Remove(slotName);
            };
        }
    }

    private void InitializePlayerItemSlot(IPlayerItem playerItem, PlayerItemSlot slot)
    {
        var dataKey = playerItem.DescriptionKey;
        if (DataManager.Instance.GetData(dataKey) is not ItemDescriptionData descriptionData)
            return;

        slot.InitializeSlot();
        slot.DescriptionData = descriptionData;
        SaveDescriptionData(playerItem.SlotName, descriptionData);

        var canEquip = playerItem.CanEquip;
        if (canEquip)
        {
            var weaponSlot = slot as WeaponSlot;
            SetUpWeaponSlot(playerItem, weaponSlot);
        }
    }

    private void SaveDescriptionData(ItemType slotName, ItemDescriptionData itemDescriptionData)
    {
        var saveDescriptionDic = _playerInventoryData.DescriptionDataDictionary;
        if (!saveDescriptionDic.ContainsKey(slotName))
            saveDescriptionDic.Add(slotName, itemDescriptionData);
    }

    private void SaveWeaponData(ItemType slotName, PlayerWeaponData playerWeaponData)
    {
        var saveWeaponDataDic = _playerInventoryData.WeaponDataDictionary;
        if(!saveWeaponDataDic.ContainsKey(slotName))
            saveWeaponDataDic.Add(slotName, playerWeaponData);
    }

    private void SetUpWeaponSlot(IPlayerItem playerItem, WeaponSlot slot)
    {
        if (playerItem is not IPlayerWeaponItem weaponItem)
            return;

        var dataKey = weaponItem.WeaponDataKey;

        WeaponPool.Instance.CreatePool(weaponItem.SlotName);
        if (DataManager.Instance.GetData(dataKey) is not PlayerWeaponData weaponData)
            return;

        slot.WeaponData = weaponData;
        SaveWeaponData(playerItem.SlotName, weaponData);
    }
    #endregion
}
