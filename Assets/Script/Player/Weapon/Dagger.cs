using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class Dagger : PlayerWeapon
{
    public override string AddressableKey => AddressablesKey.Prefab_PlayerDagger;
    public override void UseWeapon()
    {
        FindTarget();
    }
}
