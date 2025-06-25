using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class PickupItem_Hammer : WeaponItem
    {
        public override string WeaponDataKey => DataKey.Hammer_Data.ToString();
        public override string DescriptionKey => DataKey.Hammer_Description.ToString();
        public override string AddressableKey => AddressablesKey.Prefab_PlayerHammer;
        public override ItemType SlotName => ItemType.Hammer;
    }
}

