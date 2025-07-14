using EnumCollection;
using GameData;

namespace ItemComponent
{
    public class PickupItem_Dagger : WeaponItem
    {
        public override ItemType SlotName => ItemType.Dagger;

        public override void StartWeapon()
        {
            if (ItemDataSO == null)
                return;

            var weaponDataSO = ItemDataSO as PlayerWeaponDataSO;
            if (weaponDataSO != null)
            {
                var weaponData = weaponDataSO.WeaponData;
                WeaponManager.Instance.SetWeapon(SlotName, weaponData);
            }
        }
    }
}

