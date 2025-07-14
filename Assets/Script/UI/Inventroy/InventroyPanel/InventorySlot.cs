using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using EnumCollection;

namespace InventoryUI
{
    public class InventorySlot : PlayerItemSlot
    {
        [Header("ItemName"), SerializeField]
        private ItemType _itemName;

        [Header("ItemUI"), SerializeField]
        private GameObject _itemUI;

        public InventoryItemData InventoryItemData { get; set; }

        public void OnItemUI()
        {
            if (InventoryItemData == null)
                return;

            _itemUI.SetActive(true);
        }

        public override void InitializeSlot() { }
        
        protected virtual void Awake()
        {
            InventoryManager.Instance.RegisterSlot(_itemName, this);
        }
    }
}

