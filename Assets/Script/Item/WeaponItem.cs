using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public abstract class WeaponItem : Item, IPlayerWeaponItem
    {
        public override bool CanEquip => true;
        public abstract string WeaponDataKey { get; }
        public abstract string AddressableKey { get; }

        public override void Interact()
        {
            InventoryManager.Instance.SetGameItem(this);
            DisableObejct();
        }
    }
}

