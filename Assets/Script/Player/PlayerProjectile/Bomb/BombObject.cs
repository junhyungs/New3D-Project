using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class BombObject : PlayerProjectile
{
    protected override void Awake()
    {
        base.Awake();
        _objectKey = ObjectKey.PlayerBombPrefab;
    }
}
