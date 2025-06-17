using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PlayerAttackState : PlayerState
    {
        public PlayerAttackState(Player player) : base(player)
        {
            Initialize(player);
        }

        protected Animator _animator;
        protected MonoBehaviour _monobehaviour;
        protected Rigidbody _rigidbody;
        protected Transform _playerTransform;
        protected WeaponObjectController _weaponObjectController;
        
        private void Initialize(Player player)
        {
            _animator = player.GetComponentInChildren<Animator>();
            _rigidbody = player.GetComponent<Rigidbody>();
            _monobehaviour = player;
            _playerTransform = player.transform;
        }

        protected void LookAtCursor()
        {
            Vector3 lookPos = new Vector3(_plane.Point.x,
                _playerTransform.position.y, _plane.Point.z);

            var distance = Vector3.Distance(_playerTransform.position, lookPos);
            if(distance > 0.1f)
                _playerTransform.LookAt(lookPos);
        }

        protected void SetWeaponController()
        {
            var iWeapon = GetCurrentWeapon();
            if(iWeapon != null)
                _weaponObjectController = iWeapon.GetWeaponController();
        }

        protected IWeapon GetCurrentWeapon()
        {
            var iWeapon = WeaponManager.Instance.CurrentWeapon;
            if(iWeapon != null)
                return iWeapon;
            return null;
        }

        protected void SwitchWeapon(PlayerHand hand)
        {
            _weaponObjectController.DeActiveCurrentWeapon();
            _weaponObjectController.SetWeaponActive(hand);
        }
    }
}

