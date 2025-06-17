using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

public class Sword : PlayerWeapon
{
    private void Start()
    {
        GetWeaponData(DataKey.Sword_Data);
    }

    public override void UseWeapon()
    {
        FindTarget();
    }
}
