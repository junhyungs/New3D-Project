using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class PickupItem_Sword : WeaponItem
    {
        public override ItemType SlotName => ItemType.Sword;

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

