using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class ForestMotherShooter : EnemyProjectileShooter
    {
        protected override string GetProjectileAddressablesKey()
        {
            return AddressablesKey.Prefab_ForestMotherProjectile;
        }

        public override void Reload()
        {
            var projectile = _pool.GetDisableProjectile();
            projectile.transform.SetParent(_shootTransform);
            projectile.transform.localPosition = Vector3.zero;
            projectile.transform.localRotation = Quaternion.identity;
        }

        public override void Shoot()
        {
            Reload();
            var playerObject = PlayerManager.Instance.PlayerObject;
            if (_shootTransform.childCount <= 0 ||
                playerObject == null)
                return;

            var targetPos = playerObject.transform.position;    
            var projectile = _shootTransform.GetChild(0);            
            var projectileComponent = projectile.GetComponent<EnemyProjectile>();
            if (projectileComponent != null)
            {
                projectile.gameObject.SetActive(true);
                projectile.transform.parent = null;
                projectileComponent.SetupProjectile(targetPos);
            }
            else
                AllDisableProjectile();
        }
    }
}

