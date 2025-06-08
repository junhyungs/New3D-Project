using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryUI
{
    public abstract class Slot : MonoBehaviour
    {
        protected ItemDescriptionData _descriptionData;
        public abstract ItemDescriptionData DescriptionData { get; set; }
    }

    public abstract class WeaponDataSlot : Slot
    {
        protected PlayerWeaponData _weaponData;
        public abstract PlayerWeaponData WeaponData { get; set; }
    }
}

