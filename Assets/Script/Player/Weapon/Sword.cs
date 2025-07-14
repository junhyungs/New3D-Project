using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

public class Sword : PlayerWeapon
{
    public override string AddressableKey => AddressablesKey.Prefab_PlayerSword;
    public override void UseWeapon()
    {
        FindTarget();
    }
}
