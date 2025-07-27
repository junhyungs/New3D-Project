using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    [System.Serializable]
    public class ShootTransformInfo
    {
        [Header("ShootTransform")]
        public Transform ShootTransform;
    }

    public interface IDisableProjectile
    {
        void Disable();
    }

    public abstract class EnemyProjectile : MonoBehaviour, IDisableProjectile
    {
        [Header("DataSO")]
        [SerializeField] protected EnemyProjectileDataSO _dataSO;

        private EnemyProjectilePool _pool;
        protected Vector3 _direction;
        protected bool _isMove;
        protected int _damage;

        public void Disable() =>
            gameObject.SetActive(false);

        public void SetEnemyProjectilePool(EnemyProjectilePool pool) =>
            _pool = pool;

        public void ReturnProjectile() =>
            _pool.ReturnProjectile(gameObject);

        public T AsData<T>()
            where T : EnemyDataSO
        {
            return _dataSO as T;
        }

        private void Awake()
        {
            OnAwakeProjectile();
        }

        private void OnEnable()
        {
            OnEnableProjectile();
        }

        private void Update()
        {
            if (!_isMove)
                return;

            OnUpdateProjectile();
        }

        private void OnTriggerEnter(Collider other)
        {
            _isMove = false;
            OnTriggerEnterProjectile(other);
        }

        protected virtual void OnAwakeProjectile() { }
        protected virtual void OnEnableProjectile() { }
        protected virtual void OnTriggerEnterProjectile(Collider other)
        {
            if (!other.TryGetComponent(out ITakeDamage itakeDamage))
                return;

            itakeDamage.TakeDamage(_damage);
            ReturnProjectile();
        }
        protected abstract void OnUpdateProjectile();
        public abstract void SetupProjectile(int damage, Vector3 direction);
    }
}

