using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class MageAnimationEvent : MonoBehaviour, IEnemyProjectilePool
    {
        [Header("ShootTransform")]
        [SerializeField] private ShootTransformInfo _info;
        private Mage _mage;

        public EnemyProjectilePool Pool { get; private set; }

        private void Awake()
        {
            _mage = GetComponent<Mage>();
        }

        private async void Start()
        {
            var factory = EnemyProjectilePoolFactory.Instance;
            if (factory != null)
                Pool = await factory.CreateEnemyProjectilePoolAsync(transform,
                    AddressablesKey.Prefab_MageProjectile);
        }

        public void Reload()
        {
            if (_info == null)
                return;

            var projectile = Pool.GetProjectile();
            projectile.transform.SetParent(_info.ShootTransform);
            projectile.transform.localPosition = Vector3.zero;
            projectile.transform.localRotation = Quaternion.identity;
        }

        public void Shoot()
        {
            if (_info.ShootTransform.childCount <= 0)
                return;

            var projectile = _info.ShootTransform.GetChild(0);
            var mageProjectileComponent = projectile.GetComponent<MageProjectile>();
            if (mageProjectileComponent != null)
            {                
                mageProjectileComponent.SetupProjectile(_mage.Property.Data.Damage, _info.ShootTransform.forward);
            }
            else
                Pool.AllDisableProjectile();
        }
    }
}

