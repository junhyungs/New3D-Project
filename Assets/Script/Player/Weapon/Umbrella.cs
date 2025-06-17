using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class Umbrella : PlayerWeapon
{
    private void Start()
    {
        GetWeaponData(DataKey.Umbrella_Data);
    }

    public override void UseWeapon()
    {
        FindTarget();
    }

}
