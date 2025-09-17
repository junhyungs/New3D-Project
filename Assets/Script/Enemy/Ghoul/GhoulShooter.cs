using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class GhoulShooter : EnemyProjectileShooter
    {
        public override void Reload()
        {
            var projectile = _pool.GetProjectile();
            projectile.transform.SetParent(_shootTransform);
            projectile.transform.localPosition = Vector3.zero;
        }

        protected override string GetProjectileAddressablesKey()
        {
            return AddressablesKey.Prefab_GhoulProjectile;
        }
    }
}

