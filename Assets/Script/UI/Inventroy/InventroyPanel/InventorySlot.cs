using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using EnumCollection;

namespace InventoryUI
{
    public class InventorySlot : Slot
    {
        [Header("ItemName"), SerializeField]
        private InventoryItem _itemName;

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

        protected virtual void Awake()
        {
            InventroyManager.Instance.RegisterSlot(_itemName, this);
        }
    }
}

