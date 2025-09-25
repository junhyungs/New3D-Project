using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class EnemyProjectilePool : MonoBehaviour
    {
        private EnemyProjectileContainer _container;

        public void Setcontainer(EnemyProjectileContainer container) =>
            _container = container;

        public void AllDisableProjectile() =>
            _container.AllDisable();

        public void ReturnProjectile(GameObject projectile)
        {
            projectile.transform.SetParent(_container.ContainerTransform);
            projectile.SetActive(false);
            _container.Enqueue(projectile);
        }

        public GameObject GetEnableProjectile()
        {
            var projectile = GetProjectile();
            projectile.SetActive(true);
            projectile.transform.parent = null;
            return projectile;
        }

        public GameObject GetDisableProjectile()
        {
            var projectile = GetProjectile();
            projectile.transform.parent = null;
            return projectile;
        }

        private GameObject GetProjectile()
        {
            var projectile = _container.Dequeue();
            if (projectile == null)
            {
                var saveItem = _container.SaveItem;
                projectile = Instantiate(saveItem, _container.ContainerTransform);
                _container.Enqueue(projectile);
            }
            return projectile;
        }
    }
}

