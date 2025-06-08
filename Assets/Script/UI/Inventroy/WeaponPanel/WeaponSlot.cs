using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace InventoryUI
{
    public class WeaponSlot : WeaponDataSlot
    {
        [Header("ItemName"), SerializeField]
        private InventoryItem _itemName;

        public override PlayerWeaponData WeaponData { get; set; }
        public override ItemDescriptionData DescriptionData { get; set; }

        private void Awake()
        {
            InventroyManager.Instance.RegisterSlot(_itemName, this);
        }


    }
}

