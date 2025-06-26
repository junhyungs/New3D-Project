using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

public class Sword : PlayerWeapon
{
    public override string AddressableKey => AddressablesKey.Prefab_PlayerSword;

    private void Start()
    {
        GetWeaponData(DataKey.Sword_Data);
    }

    public override void UseWeapon()
    {
        FindTarget();
    }
}
