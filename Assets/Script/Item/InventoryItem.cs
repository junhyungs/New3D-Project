using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class InventoryItem : Item
    {
        [Header("SlotName"), SerializeField]
        private ItemType _slotName;
        [Header("DataKey"), SerializeField]
        private DataKey _descriptionKey;

        public override bool CanEquip => false;
        public override ItemType SlotName => _slotName;
        public override string DescriptionKey => _descriptionKey.ToString();

        public override void Interact()
        {
            InventoryManager.Instance.SetGameItem(this);
            DisableObejct();
        }
    }
}

