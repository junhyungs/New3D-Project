using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class InventoryItemDataSO : ScriptableObject
    {
        [Header("ItemName"), SerializeField]
        private string _itemName;
        [Header("ItemDescription"), SerializeField, TextArea]
        private string _itemDescription;
        [Header("Equip"), SerializeField]
        private bool _equip;

        public string Name => _itemName;
        public string ItemDescription => _itemDescription;
        public bool Equip => _equip;
    }
}

