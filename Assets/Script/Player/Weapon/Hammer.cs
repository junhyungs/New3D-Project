using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class Hammer : PlayerWeapon
{
    public override string AddressableKey => AddressablesKey.Prefab_PlayerHammer;
    public override void UseWeapon()
    {
        FindTarget();
    }
}
