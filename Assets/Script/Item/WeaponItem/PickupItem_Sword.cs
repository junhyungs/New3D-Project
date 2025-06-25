using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class PickupItem_Sword : WeaponItem
    {
        public override string WeaponDataKey => DataKey.Sword_Data.ToString();
        public override string DescriptionKey => DataKey.Sword_Description.ToString();
        public override string AddressableKey => AddressablesKey.Prefab_PlayerSword;
        public override ItemType SlotName => ItemType.Sword;
    }
}

