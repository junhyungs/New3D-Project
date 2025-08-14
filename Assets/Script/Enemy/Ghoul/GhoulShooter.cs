using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class GhoulShooter : EnemyProjectileShooter
    {
        protected override string GetProjectileAddressablesKey()
        {
            return AddressablesKey.Prefab_GhoulProjectile;
        }
    }
}

