using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public interface IEnemyProjectilePool
    {
        EnemyProjectilePool Pool { get; }
    }
    
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

        public GameObject GetProjectile()
        {
            var projectile = _container.Dequeue();
            if(projectile == null)
            {
                var saveItem = _container.SaveItem;
                projectile = Instantiate(saveItem, _container.ContainerTransform);
                _container.Enqueue(projectile);
            }

            projectile.SetActive(true);
            projectile.transform.parent = null;
            return projectile;
        }
    }
}

