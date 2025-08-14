using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

namespace EnemyComponent
{
    public class MageShooter : EnemyProjectileShooter
    {
        protected override string GetProjectileAddressablesKey()
        {
            return AddressablesKey.Prefab_MageProjectile;
        }
    }
}

