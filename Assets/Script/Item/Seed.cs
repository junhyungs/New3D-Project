using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class Seed : Item, ICurrencyItem
    {
        public override ItemType SlotName => ItemType.Seed;

        public int GetValue()
        {
            var seedDataSO = ItemDataSO as SeedItemDataSO;
            if (seedDataSO == null)
                return 1;

            return seedDataSO.Count;
        }

        public override void Interact()
        {
            InventoryManager.Instance.SetItem(this);
            DisableObejct();
        }
    }
}

