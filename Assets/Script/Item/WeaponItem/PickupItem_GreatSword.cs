using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class PickupItem_GreatSword : WeaponItem
    {
        public override string WeaponDataKey => DataKey.GreatSword_Data.ToString();
        public override string DescriptionKey => DataKey.GreatSword_Description.ToString();
        public override string AddressableKey => AddressablesKey.Prefab_PlayerGreatSword;
        public override ItemType SlotName => ItemType.GreatSword;
    }
}

