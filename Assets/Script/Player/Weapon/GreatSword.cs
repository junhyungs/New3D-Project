using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class GreatSword : PlayerWeapon
{
    private void Start()
    {
        GetWeaponData(DataKey.GreatSword_Data);
    }

    public override void UseWeapon()
    {
        FindTarget();
    }
}
