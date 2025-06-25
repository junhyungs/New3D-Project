using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class Umbrella : PlayerWeapon
{
    public override string AddressableKey => AddressablesKey.Prefab_PlayerUmbrella;

    private void Start()
    {
        GetWeaponData(DataKey.Umbrella_Data);
    }

    public override void UseWeapon()
    {
        FindTarget();
    }

}
