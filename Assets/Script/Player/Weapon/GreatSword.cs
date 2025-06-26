using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class GreatSword : PlayerWeapon
{
    public override string AddressableKey => AddressablesKey.Prefab_PlayerGreatSword;

    private void Start()
    {
        GetWeaponData(DataKey.GreatSword_Data);
    }

    public override void UseWeapon()
    {
        FindTarget();
    }
}
