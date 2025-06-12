using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour, IWeapon
{
    protected GameObject[] _weaponArray;

    public void InitializeWeapon(GameObject[] weaponArray)
    {
        _weaponArray = weaponArray;
    }

    public abstract void UseWeapon();
}
