using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ItemComponent
{
    public class PickupItem_Umbrella : WeaponItem
    {
        public override string WeaponDataKey => DataKey.Umbrella_Data.ToString();
        public override string DescriptionKey => DataKey.Umbrella_Description.ToString();
        public override string AddressableKey => AddressablesKey.Prefab_PlayerUmbrella;
        public override ItemType SlotName => ItemType.Umbrella;
    }
}

