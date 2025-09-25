using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
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
        protected float _returnTime = 6f;

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
        protected virtual void OnEnableProjectile()
        {
            Invoke(nameof(ReturnProjectile), _returnTime);
        }
        protected virtual void OnTriggerEnterProjectile(Collider other)
        {
            if (!other.TryGetComponent(out ITakeDamage itakeDamage))
                return;

            itakeDamage.TakeDamage(_damage);
            ReturnProjectile();
        }
        protected virtual void OnUpdateProjectile() { }
        public virtual void SetupProjectile(int damage, Vector3 direction) { }
        public virtual void SetupProjectile(Vector3 direction) { }
    }
}

