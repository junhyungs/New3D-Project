using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public abstract class EnemyProjectileShooter : MonoBehaviour
    {
        [Header("ShootTransform"), SerializeField]
        protected Transform _shootTransform;
        protected EnemyProjectilePool _pool;
        public int Damage { get; set; }
        
        private async void Start()
        {
            var factory = EnemyProjectilePoolFactory.Instance;
            if (factory != null)
                _pool = await factory.CreateEnemyProjectilePoolAsync(transform,
                    GetProjectileAddressablesKey());
        }

        protected abstract string GetProjectileAddressablesKey();

        public virtual void Reload()
        {
            var projectile = _pool.GetProjectile();
            projectile.transform.SetParent(_shootTransform);
            projectile.transform.localPosition = Vector3.zero;
            projectile.transform.localRotation = Quaternion.identity;
        }

        public virtual void Shoot()
        {
            if (_shootTransform.childCount <= 0)
                return;

            var projectile = _shootTransform.GetChild(0);
            var projectileComponent = projectile.GetComponent<EnemyProjectile>();
            if (projectileComponent != null)
                projectileComponent.SetupProjectile(Damage,
                    _shootTransform.forward);
            else
                AllDisableProjectile();
        }

        public void AllDisableProjectile()
        {
            _pool.AllDisableProjectile();
        }
    }
}

