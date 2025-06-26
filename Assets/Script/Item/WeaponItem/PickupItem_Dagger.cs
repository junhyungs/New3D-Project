using EnumCollection;

namespace ItemComponent
{
    public class PickupItem_Dagger : WeaponItem
    {
        public override string WeaponDataKey => DataKey.Dagger_Data.ToString();
        public override string DescriptionKey => DataKey.Dagger_Description.ToString();
        public override string AddressableKey => AddressablesKey.Prefab_PlayerDagger;
        public override ItemType SlotName => ItemType.Dagger;
    }
}

