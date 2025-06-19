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

        public override ItemDescriptionData DescriptionData
        {
            get => _descriptionData;
            set
            {
                if(!_itemUI.activeSelf)
                    _itemUI.SetActive(true);

                _descriptionData = value;
            }
        }

        public override void InitializeSlot() { }
        
        protected virtual void Awake()
        {
            InventoryManager.Instance.RegisterSlot(_itemName, this);
        }
    }
}

