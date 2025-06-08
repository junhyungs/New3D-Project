using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using InventoryUI;

public class InventroyManager : Singleton_MonoBehaviour<InventroyManager>
{
    private Dictionary<InventoryItem, Slot> _slotDictionary = new Dictionary<InventoryItem, Slot>();

    private void Awake()
    {
        DataManager.Instance.TestLoadItemDescriptionData(); //테스트 코드
    }

    public void RegisterSlot(InventoryItem item, Slot slot)
    {
        if(!_slotDictionary.ContainsKey(item))
            _slotDictionary.Add(item, slot);
    }

    public void SetItem()
    {

    }
}
