using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class Dagger : PlayerWeapon
{
    private void Start()
    {
        GetWeaponData(DataKey.Dagger_Data);
    }

    public override void UseWeapon()
    {
        FindTarget();
    }
}
