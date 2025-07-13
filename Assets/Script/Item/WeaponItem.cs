using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public abstract class WeaponItem : Item
    {
        public abstract void StartWeapon();
        public override void Interact()
        {
            InventoryManager.Instance.SetItem(this);
            DisableObejct();
        }
    }
}

