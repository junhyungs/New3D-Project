using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class ItemDataSO : ScriptableData
    {
        [Header("Item Name"), SerializeField]
        private string _itemName;
        [Header("Item Description"), SerializeField, TextArea]
        private string _itemDescription;
        [Header("Equip"), SerializeField]
        private bool _equip;

        public string ItemName => _itemName;
        public string ItemDescription => _itemDescription;
        public bool Equip => _equip;
        public override ScriptableDataKey Key => _key;
    }
}

