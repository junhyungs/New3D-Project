using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class TrinketItem : Item, IPlayerItem
    {
        [Header("SlotName"), SerializeField]
        private ItemType _slotName;
        [Header("DataKey"), SerializeField]
        private DataKey _descriptionKey;

        public bool CanEquip => false;
        public ItemType SlotName => _slotName;
        public string DescriptionKey => _descriptionKey.ToString();

        public override void Interact()
        {
            InventroyManager.Instance.SetItem(this);
            DisableObejct();
        }
    }
}

