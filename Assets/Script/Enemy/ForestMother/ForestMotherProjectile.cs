using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class ForestMotherProjectile : EnemyProjectile
    {
        [Header("ParticleSystem")]
        [SerializeField] private ParticleSystem _fire;
        [SerializeField] private ParticleSystem _explosion;
        [Header("Body")]
        [SerializeField] private GameObject _body;

        private Rigidbody _rigidBody;
        private ForestMotherProjectileSO _data;

        protected override void OnAwakeProjectile()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _data = _dataSO as ForestMotherProjectileSO;
            _returnTime = 10f;
        }

        protected override void OnEnableProjectile()
        {
            BodySetting(true);
        }

        private void BodySetting(bool value)
        {
            Action action = value == true ?
                _fire.Play : _fire.Stop;

            action.Invoke();
            _body.SetActive(value);
        }

        public override void SetupProjectile(Vector3 direction)
        {
            var targetDirection = (direction - transform.position).normalized;
            var forceVector = new Vector3(targetDirection.x , targetDirection.y + 3f,
                targetDirection.z);

            _rigidBody.useGravity = true;
            _rigidBody.AddForce(forceVector * _dataSO.Speed, ForceMode.Impulse);
        }

        protected override void OnTriggerEnterProjectile(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
                return;

            BodySetting(false);

            _explosion.Play();
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.useGravity = false;

            var results = new Collider[1];
            var playerLayer = LayerMask.GetMask("Player");
            var hitCount = Physics.OverlapSphereNonAlloc(transform.position,
                _data.ExplosionRadius, results, playerLayer);
            if(hitCount > 0)
            {
                var playerObject = results[0].gameObject;
                if (!playerObject.TryGetComponent(out ITakeDamage itakeDamage))
                    return;

                itakeDamage.TakeDamage(_damage);
            }

            StartCoroutine(AfterCoroutine());
        }

        private IEnumerator AfterCoroutine()
        {
            _fire.Stop();
            yield return new WaitWhile(() => _explosion.isPlaying);

            ReturnProjectile();
        }
    }
}

