using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class InventoryItem : Item, IInventoryItem
    {
        [Header("SlotName"), SerializeField]
        private ItemType _slotName;
        public override ItemType SlotName => _slotName;

        public override void Interact()
        {
            InventoryManager.Instance.SetItem(this);
            DisableObejct();
        }
    }
}

