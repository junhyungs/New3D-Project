using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class GreatSword : PlayerWeapon
{
    public override string AddressableKey => AddressablesKey.Prefab_PlayerGreatSword;
    public override void UseWeapon()
    {
        FindTarget();
    }
}
