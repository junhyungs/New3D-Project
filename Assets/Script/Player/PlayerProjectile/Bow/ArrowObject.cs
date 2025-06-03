using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class ArrowObject : PlayerProjectile
    {
        [Header("FireParticleObject"), SerializeField] private GameObject _fireParticleObject;
        private bool _isBurning;

        protected override void Awake()
        {
            base.Awake();
            _objectKey = ObjectKey.PlayerArrowPrefab;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ActiveMyParticle(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            Ignite(other);
            Hit(other);
        }

        protected override void Hit(Collider other)
        {
            base.Hit(other);
            ReturnObjectPool();
        }

        private void Ignite(Collider other)
        {
            if (!other.TryGetComponent(out IBurnable burnable))
                return;

            bool otherIsBurning = burnable.IsBurning();

            if (otherIsBurning && !_isBurning)
                ActiveMyParticle(true);
            else if (!otherIsBurning && _isBurning)
                burnable.Ignite();
        }

        private void ActiveMyParticle(bool enable)
        {
            _isBurning = enable;
            _fireParticleObject.SetActive(enable);
        }
    }
}

