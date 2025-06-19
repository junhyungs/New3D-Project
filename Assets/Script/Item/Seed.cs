using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class Seed : CurrencyItem
    {
        public override ItemType SlotName => ItemType.Seed;

        public override int GetValue()
        {
            return 1;
        }

        public override void Interact()
        {
            _collider.enabled = false;
            InventoryManager.Instance.SetGameItem(this);
            gameObject.SetActive(false);
        }
    }
}

